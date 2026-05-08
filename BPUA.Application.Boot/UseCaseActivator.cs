using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;
using BPUA.Core;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Activates use-case assemblies on demand by reusing the existing boot-time assembly loading pipeline.
    /// The class also prevents duplicate concurrent activations of the same use case and stores
    /// an activation stamp in the service registry so subsequent calls can return immediately.
    /// </summary>
    public sealed class UseCaseActivator : IUseCaseActivator
    {
        /// <summary>
        /// Stores one in-flight activation operation per normalized use-case key.
        /// The Lazy wrapper ensures that only one activation result is computed
        /// for a given key even when multiple callers request activation simultaneously.
        /// </summary>
        readonly ConcurrentDictionary<string, Lazy<UseCaseActivationResult>> _singleFlight
            = new ConcurrentDictionary<string, Lazy<UseCaseActivationResult>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Activates the assemblies required for the specified use case.
        /// The method first checks whether the use case has already been activated and stamped
        /// in the service registry. If it has, the stored route is returned immediately.
        /// Otherwise the method ensures that only one activation operation runs for the same
        /// normalized use-case key and then delegates the actual loading work to ActivateCore.
        /// The public contract remains asynchronous for compatibility, but the work itself is synchronous.
        /// </summary>
        /// <param name="identifier">The BPU identifier describing the requested use case.</param>
        /// <param name="serviceRegistry">The service registry used to store and query activation stamps and registered services.</param>
        /// <returns>
        /// A task that contains a UseCaseActivationResult describing whether activation succeeded,
        /// whether anything new had to be loaded, and what the default route for the activated use case is.
        /// </returns>
        public Task<UseCaseActivationResult> ActivateAsync(IBpuIdentifier identifier, IServiceRegistry serviceRegistry)
        {
            if (identifier == null)
            {
                return Task.FromResult(Failure("Identifier is null."));
            }

            if (serviceRegistry == null)
            {
                return Task.FromResult(Failure("Service registry is null."));
            }

            string normalizedActivationKey = NormalizeActivationKey(identifier);
            string activationStampKey = BuildStampKey(normalizedActivationKey);

            object? registeredObject;
            bool stampFound = serviceRegistry.TryGetRegisteredObject(activationStampKey, out registeredObject);
            if (stampFound)
            {
                UseCaseActivationStamp? activationStamp = registeredObject as UseCaseActivationStamp;
                if (activationStamp != null)
                {
                    UseCaseActivationResult alreadyActivatedResult = new UseCaseActivationResult();
                    alreadyActivatedResult.Succeeded = true;
                    alreadyActivatedResult.NoAdditionalAssembliesWereLoaded = true;
                    alreadyActivatedResult.DefaultRoute = activationStamp.DefaultRoute;
                    return Task.FromResult(alreadyActivatedResult);
                }
            }

            ActivationCoreInvoker activationCoreInvoker = new ActivationCoreInvoker(this, identifier, serviceRegistry);
            Lazy<UseCaseActivationResult> newLazyResult = new Lazy<UseCaseActivationResult>(
                activationCoreInvoker.Invoke,
                true);

            Lazy<UseCaseActivationResult> lazyResult = _singleFlight.GetOrAdd(normalizedActivationKey, newLazyResult);

            UseCaseActivationResult activationResult;
            try
            {
                activationResult = lazyResult.Value;
            }
            catch (Exception exception)
            {
                _singleFlight.TryRemove(normalizedActivationKey, out _);
                throw new InvalidOperationException(
                    BuildActivationDiagnosticMessage(
                        "Use case activation failed while executing the single-flight activation operation.",
                        identifier,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        normalizedActivationKey),
                    exception);
            }

            if (activationResult == null)
            {
                _singleFlight.TryRemove(normalizedActivationKey, out _);
                return Task.FromResult(Failure("Activation returned no result."));
            }

            if (!activationResult.Succeeded)
            {
                _singleFlight.TryRemove(normalizedActivationKey, out _);
                return Task.FromResult(activationResult);
            }

            string defaultRoute = activationResult.DefaultRoute;
            if (string.IsNullOrEmpty(defaultRoute))
            {
                defaultRoute = ComputeDefaultRoute(identifier);
            }

            UseCaseActivationStamp activationStampToRegister = new UseCaseActivationStamp();
            activationStampToRegister.DefaultRoute = defaultRoute;
            activationStampToRegister.TimestampUtc = DateTime.UtcNow;

            serviceRegistry.TryRegisterObject(activationStampKey, activationStampToRegister);

            activationResult.DefaultRoute = defaultRoute;
            activationResult.NoAdditionalAssembliesWereLoaded = false;
            return Task.FromResult(activationResult);
        }

        /// <summary>
        /// Performs the actual activation work for a use case.
        /// The method resolves the plugin root from the already initialized application,
        /// uses the dynamic assemblies path index from the service registry to resolve the requested
        /// layer-specific assembly path, and then invokes the dynamic assembly loader
        /// using the same assembly processor list and loaded-assembly list that are used by the main boot pipeline.
        /// The method is intentionally synchronous because assembly loading and registration are already synchronous.
        /// </summary>
        /// <param name="identifier">The identifier that describes the use case to activate.</param>
        /// <param name="serviceRegistry">The registry that receives newly registered services and pages.</param>
        /// <returns>A result object describing success or failure of the activation operation.</returns>
        public UseCaseActivationResult ActivateCore(IBpuIdentifier identifier, IServiceRegistry serviceRegistry)
        {
            string pluginRoot = string.Empty;
            string useCaseFolder = string.Empty;
            string relativeAssemblyPath = string.Empty;
            string pathToDynamicAssembly = string.Empty;

            try
            {
                IBpuaApplication application = BpuaApplication.GetInstance();
                pluginRoot = application.PathToFolderWithDynamicAssemblies;
                if (string.IsNullOrEmpty(pluginRoot))
                {
                    return Failure("Path to folder with dynamic assemblies is not configured.");
                }

                string assemblyFileName = BuildDynamicAssemblyFileName(identifier);
                if (string.IsNullOrEmpty(assemblyFileName))
                {
                    return Failure("Unable to resolve assembly file name for the requested use case layer.");
                }

                string assemblyName = Path.GetFileNameWithoutExtension(assemblyFileName);
                if (!serviceRegistry.TryGetRegisteredDynamicAssemblyPath(assemblyName, out relativeAssemblyPath))
                {
                    return Failure(
                        BuildActivationDiagnosticMessage(
                            "Use case assembly is not registered in dynamic assemblies path index.",
                            identifier,
                            pluginRoot,
                            string.Empty,
                            string.Empty,
                            string.Empty));
                }

                pathToDynamicAssembly = Path.Combine(pluginRoot, relativeAssemblyPath);
                useCaseFolder = Path.GetDirectoryName(pathToDynamicAssembly) ?? string.Empty;

                if (!File.Exists(pathToDynamicAssembly))
                {
                    return Failure("Use case assembly does not exist: " + pathToDynamicAssembly);
                }

                DynamicAssembliesLoader dynamicAssembliesLoader = new DynamicAssembliesLoader();
                List<Assembly> localLoadedAssemblies = new List<Assembly>();
                Assembly? loadedAssembly = dynamicAssembliesLoader.LoadDynamicAssembly(
                    pathToDynamicAssembly,
                    serviceRegistry,
                    localLoadedAssemblies,
                    ListOfAssemblyProcessors);
                if (loadedAssembly == null)
                {
                    return Failure("Failed to load use case assembly: " + pathToDynamicAssembly);
                }

                dynamicAssembliesLoader.LoadDynamicTransitionAssembliesFromFolder(
                    useCaseFolder,
                    serviceRegistry,
                    localLoadedAssemblies,
                    ListOfAssemblyProcessors);

                UseCaseActivationResult successResult = new UseCaseActivationResult();
                successResult.Succeeded = true;
                successResult.NoAdditionalAssembliesWereLoaded = false;
                successResult.DefaultRoute = ComputeDefaultRoute(identifier);
                return successResult;
            }
            catch (Exception exception)
            {
                return Failure(
                    BuildActivationDiagnosticMessage(
                        "Activation failed. " + exception,
                        identifier,
                        pluginRoot,
                        useCaseFolder,
                        relativeAssemblyPath,
                        string.Empty));
            }
        }

        /// <summary>
        /// Resolves the physical folder that contains the assemblies for the requested use case.
        /// The build layout stores each use case under an optional breadcrumbs path followed by
        /// a leaf folder composed as DomainName.UseCaseName. For example, when the plugin root is
        /// Build\PluginFolder, the breadcrumbs are Libraries\Setup\Administration, the domain is BPUA,
        /// and the use case is Account, the resulting folder must be:
        /// Build\PluginFolder\Libraries\Setup\Administration\BPUA.Account.
        /// This method therefore combines normalized breadcrumbs and the composed use-case folder name
        /// instead of choosing only one of them.
        /// </summary>
        /// <param name="pluginRoot">The root folder under which dynamic use-case assemblies are stored.</param>
        /// <param name="identifier">The identifier from which folder information is taken.</param>
        /// <returns>The resolved folder path, or an empty string when the identifier does not contain enough information.</returns>
        static string ResolveFolder(string pluginRoot, IBpuIdentifier identifier)
        {
            if (string.IsNullOrEmpty(pluginRoot))
            {
                return string.Empty;
            }

            string normalizedBreadcrumbs = string.Empty;
            if (!string.IsNullOrEmpty(identifier.Breadcrumbs))
            {
                normalizedBreadcrumbs = Breadcrumbs.NormalizeBreadcrumbs(identifier.Breadcrumbs);
            }

            string folderLeaf = BuildUseCaseFolderLeaf(identifier);
            if (string.IsNullOrEmpty(folderLeaf))
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(normalizedBreadcrumbs))
            {
                return Path.Combine(pluginRoot, folderLeaf);
            }

            string breadcrumbsAsPath = normalizedBreadcrumbs.Replace('/', Path.DirectorySeparatorChar);
            return Path.Combine(pluginRoot, breadcrumbsAsPath, folderLeaf);
        }

        /// <summary>
        /// Builds the leaf folder name used by the build pipeline for a specific use case.
        /// The preferred shape is DomainName.UseCaseName because that matches the output folder name
        /// produced by the build and deployment pipeline. When the domain is not available,
        /// the method falls back to the use-case name alone.
        /// </summary>
        /// <param name="identifier">The identifier that supplies domain and use-case values.</param>
        /// <returns>The folder leaf name, or an empty string when no usable use-case information exists.</returns>
        static string BuildUseCaseFolderLeaf(IBpuIdentifier identifier)
        {
            string useCaseName = string.Empty;
            if (!string.IsNullOrEmpty(identifier.UseCaseName))
            {
                useCaseName = identifier.UseCaseName.Trim();
            }

            if (string.IsNullOrEmpty(useCaseName))
            {
                return string.Empty;
            }

            string domainName = string.Empty;
            if (!string.IsNullOrEmpty(identifier.DomainName))
            {
                domainName = identifier.DomainName.Trim();
            }

            if (string.IsNullOrEmpty(domainName))
            {
                return useCaseName;
            }

            return domainName + "." + useCaseName;
        }

        /// <summary>
        /// Builds the full path to the layer-specific dynamic assembly requested by the identifier.
        /// The expected assembly naming convention is DomainName.UseCaseName.ApplicationLayerName.dll.
        /// </summary>
        /// <param name="useCaseFolder">Resolved use-case folder.</param>
        /// <param name="identifier">Identifier that supplies domain, use-case, and layer names.</param>
        /// <returns>Full path to the requested assembly, or an empty string when required identifier data is missing.</returns>
        static string BuildPathToDynamicAssembly(string useCaseFolder, IBpuIdentifier identifier)
        {
            if (string.IsNullOrEmpty(useCaseFolder))
            {
                return string.Empty;
            }

            string assemblyFileName = BuildDynamicAssemblyFileName(identifier);
            if (string.IsNullOrEmpty(assemblyFileName))
            {
                return string.Empty;
            }

            return Path.Combine(useCaseFolder, assemblyFileName);
        }

        /// <summary>
        /// Builds the layer-specific assembly file name requested by the identifier.
        /// </summary>
        /// <param name="identifier">Identifier that supplies domain, use-case, and layer names.</param>
        /// <returns>Assembly file name, or an empty string when required identifier data is missing.</returns>
        static string BuildDynamicAssemblyFileName(IBpuIdentifier identifier)
        {
            string folderLeaf = BuildUseCaseFolderLeaf(identifier);
            if (string.IsNullOrEmpty(folderLeaf))
            {
                return string.Empty;
            }

            string applicationLayerName = string.Empty;
            if (!string.IsNullOrEmpty(identifier.ApplicationLayerName))
            {
                applicationLayerName = identifier.ApplicationLayerName.Trim();
            }

            if (string.IsNullOrEmpty(applicationLayerName))
            {
                return string.Empty;
            }

            return folderLeaf + "." + applicationLayerName + ".dll";
        }

        /// <summary>
        /// Computes the default route for a use case.
        /// The use-case name is preferred unless it is empty or represents the generic Application node.
        /// In that case the last breadcrumbs segment is used instead.
        /// </summary>
        /// <param name="identifier">The identifier used to determine the route segment.</param>
        /// <returns>The normalized default route for the activated use case.</returns>
        static string ComputeDefaultRoute(IBpuIdentifier identifier)
        {
            string routeSegment = string.Empty;

            if (!string.IsNullOrEmpty(identifier.UseCaseName) &&
                !identifier.UseCaseName.Equals("Application", StringComparison.OrdinalIgnoreCase))
            {
                routeSegment = identifier.UseCaseName;
            }
            else
            {
                routeSegment = Breadcrumbs.GetBreadcrumsLastSegment(identifier.Breadcrumbs);
            }

            return "/u/" + routeSegment.ToLowerInvariant();
        }

        /// <summary>
        /// Builds a stable normalized key used to serialize activation requests for the same use case.
        /// The key combines domain and use-case information and falls back to the last breadcrumbs segment
        /// when the use-case name is empty or represents the generic Application node.
        /// </summary>
        /// <param name="identifier">The identifier from which the normalized activation key is derived.</param>
        /// <returns>A lower-cased normalized activation key.</returns>
        static string NormalizeActivationKey(IBpuIdentifier identifier)
        {
            string domainName = string.Empty;
            if (!string.IsNullOrEmpty(identifier.DomainName))
            {
                domainName = identifier.DomainName;
            }

            string useCaseName = string.Empty;
            if (!string.IsNullOrEmpty(identifier.UseCaseName))
            {
                useCaseName = identifier.UseCaseName;
            }

            if (string.IsNullOrEmpty(useCaseName) ||
                useCaseName.Equals("Application", StringComparison.OrdinalIgnoreCase))
            {
                string breadcrumbsLeaf = Breadcrumbs.GetBreadcrumsLastSegment(identifier.Breadcrumbs);
                if (string.IsNullOrEmpty(breadcrumbsLeaf))
                {
                    useCaseName = "application";
                }
                else
                {
                    useCaseName = breadcrumbsLeaf;
                }
            }

            string combinedKey = string.Empty;
            if (string.IsNullOrEmpty(domainName))
            {
                combinedKey = useCaseName;
            }
            else
            {
                combinedKey = domainName + "." + useCaseName;
            }

            string applicationLayerName = string.Empty;
            if (!string.IsNullOrEmpty(identifier.ApplicationLayerName))
            {
                applicationLayerName = identifier.ApplicationLayerName.Trim();
            }

            if (!string.IsNullOrEmpty(applicationLayerName))
            {
                combinedKey += "." + applicationLayerName;
            }

            return combinedKey.Trim().ToLowerInvariant();
        }

        /// <summary>
        /// Builds the key under which the activation stamp is stored in the service registry.
        /// </summary>
        /// <param name="normalizedKey">The normalized use-case activation key.</param>
        /// <returns>The registry key used to store the activation stamp.</returns>
        static string BuildStampKey(string normalizedKey)
        {
            return "Activation:" + normalizedKey;
        }

        /// <summary>
        /// Builds a diagnostic message for activation failures.
        /// This method centralizes the most important identifier and folder values so exceptions and
        /// failure results carry enough context to diagnose configuration and loading problems.
        /// </summary>
        /// <param name="prefix">The leading diagnostic text.</param>
        /// <param name="identifier">The identifier associated with the activation request.</param>
        /// <param name="pluginRoot">The resolved plugin root, if known.</param>
        /// <param name="useCaseFolder">The resolved use-case folder, if known.</param>
        /// <param name="relativeAssemblyPath">The relative assembly path from the registry, if known.</param>
        /// <param name="normalizedActivationKey">The normalized activation key, if known.</param>
        /// <returns>A composed diagnostic message.</returns>
        static string BuildActivationDiagnosticMessage(
            string prefix,
            IBpuIdentifier identifier,
            string pluginRoot,
            string useCaseFolder,
            string relativeAssemblyPath,
            string normalizedActivationKey)
        {
            string message = prefix;
            message += " DomainName='" + identifier.DomainName + "'.";
            message += " UseCaseName='" + identifier.UseCaseName + "'.";
            message += " ApplicationLayerName='" + identifier.ApplicationLayerName + "'.";
            message += " StateName='" + identifier.StateName + "'.";
            message += " TransitionName='" + identifier.TransitionName + "'.";
            message += " Breadcrumbs='" + identifier.Breadcrumbs + "'.";

            if (!string.IsNullOrEmpty(normalizedActivationKey))
            {
                message += " NormalizedActivationKey='" + normalizedActivationKey + "'.";
            }

            if (!string.IsNullOrEmpty(pluginRoot))
            {
                message += " PluginRoot='" + pluginRoot + "'.";
            }

            if (!string.IsNullOrEmpty(useCaseFolder))
            {
                message += " UseCaseFolder='" + useCaseFolder + "'.";
            }

            if (!string.IsNullOrEmpty(relativeAssemblyPath))
            {
                message += " RelativeAssemblyPath='" + relativeAssemblyPath + "'.";
            }

            return message;
        }

        /// <summary>
        /// Creates a failed activation result and appends the supplied error message when one is provided.
        /// </summary>
        /// <param name="message">The error message to attach to the result.</param>
        /// <returns>A failure result object.</returns>
        static UseCaseActivationResult Failure(string message)
        {
            UseCaseActivationResult failureResult = new UseCaseActivationResult();
            failureResult.Succeeded = false;
            failureResult.NoAdditionalAssembliesWereLoaded = false;

            if (!string.IsNullOrEmpty(message))
            {
                failureResult.Errors.Add(message);
            }

            return failureResult;
        }

        /// <summary>
        /// Gets or sets the assembly processors that must be applied to each newly loaded assembly.
        /// These processors register services, pages, and any other assembly-discovered runtime artifacts.
        /// </summary>
        public List<IBpuaAssemblyProcessor> ListOfAssemblyProcessors { get; set; } = default!;

        /// <summary>
        /// Gets or sets the root path to the folder that contains dynamically loadable assemblies.
        /// The property is retained for compatibility with the broader boot infrastructure.
        /// </summary>
        public string PathToFolderWithDynamicAssemblies { get; set; } = default!;
    }
}
