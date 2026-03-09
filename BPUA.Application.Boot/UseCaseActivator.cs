using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;
using BPUA.Core;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Uses the existing Boot pipeline to load assemblies on demand.
    /// Now also provides single-flight (one-time) activation and a registry stamp
    /// so repeat calls short-circuit cheaply.
    /// </summary>
    public sealed class UseCaseActivator : IUseCaseActivator
    {
        // One activation per normalized use-case key per process
        readonly ConcurrentDictionary<string, Lazy<Task<UseCaseActivationResult>>> _singleFlight
            = new ConcurrentDictionary<string, Lazy<Task<UseCaseActivationResult>>>(StringComparer.OrdinalIgnoreCase);

        public async Task<UseCaseActivationResult> ActivateAsync(IBPUAIdentifier identifier, IServiceRegistry serviceRegistry)
        {
            if (identifier == null)
                return Failure("Identifier is null.");
            if (serviceRegistry == null)
                return Failure("Service registry is null.");

            // ----- Fast path: has this use case already been activated in this process? -----
            string normKey = NormalizeActivationKey(identifier);
            string stampKey = BuildStampKey(normKey);

            object? stampObj;
            if (serviceRegistry.TryGetRegisteredObject(stampKey, out stampObj) && stampObj is UseCaseActivationStamp stamp)
            {
                return new UseCaseActivationResult
                {
                    Succeeded = true,
                    NoAdditionalAssembliesWereLoaded = true,
                    DefaultRoute = stamp.DefaultRoute
                };
            }

            // ----- Cold path: ensure only ONE activation runs for this key (single-flight) -----
            var invoker = new ActivationCoreInvoker(this, identifier, serviceRegistry); // no lambdas
            var newLazy = new Lazy<Task<UseCaseActivationResult>>(invoker.Invoke, LazyThreadSafetyMode.ExecutionAndPublication);
            var lazy = _singleFlight.GetOrAdd(normKey, newLazy);

            UseCaseActivationResult result;
            try
            {
                result = await lazy.Value;
            }
            catch
            {
                // allow retry on next call if the activation failed
                _singleFlight.TryRemove(normKey, out _);
                throw;
            }

            if (result == null)
                return Failure("Activation returned no result.");

            if (!result.Succeeded)
                return result;

            // Compute a stable route (prefer activator-provided)
            string route = !string.IsNullOrEmpty(result.DefaultRoute)
                ? result.DefaultRoute
                : ComputeDefaultRoute(identifier);

            // Stamp the registry (first-wins) so future calls short-circuit
            serviceRegistry.TryRegisterObject(stampKey, new UseCaseActivationStamp
            {
                DefaultRoute = route,
                TimestampUtc = DateTime.UtcNow
            });

            // First success → AlreadyLoaded = false (by definition)
            result.DefaultRoute = route;
            result.NoAdditionalAssembliesWereLoaded = false;
            return result;
        }

        // ------------------------- Core activation you already had -------------------------

        async Task<UseCaseActivationResult> ActivateCoreAsync(IBPUAIdentifier identifier, IServiceRegistry serviceRegistry)
        {
            // Resolve plugin root from the already-initialized application
            IBPUAApplication app = BPUAApplication.GetInstance();
            string pluginRoot = app.PathToFolderWithDynamicAssemblies;
            if (string.IsNullOrEmpty(pluginRoot))
                return Failure("Path to folder with dynamic assemblies is not configured.");

            // Resolve the physical folder for this use case
            string folder = ResolveFolder(pluginRoot, identifier);
            if (string.IsNullOrEmpty(folder))
                return Failure("Unable to resolve folder for the requested use case.");
            if (!Directory.Exists(folder))
                return Failure("Use case folder does not exist: " + folder);

            // Your existing loader — you already call this:
            // NOTE: make sure ListOfLoadedAssemblies and ListOfAssemblyProcessors are set up externally.
            var smartAssemblyLoader = new DynamicAssembliesLoader();
            try
            {
                await Task.Run(() =>
                {
                    smartAssemblyLoader.LoadDynamicAssemblies(folder, serviceRegistry, ListOfLoadedAssemblies, ListOfAssemblyProcessors);
                });
            }
            catch (Exception ex)
            {
                return Failure("Activation failed: " + ex.Message);
            }

            // Success: the processors have registered services/pages into the registry
            return new UseCaseActivationResult
            {
                Succeeded = true,
                NoAdditionalAssembliesWereLoaded = false, // orchestration of "already" is handled by fast path/stamp
                DefaultRoute = ComputeDefaultRoute(identifier)
            };
        }

        // ----------------------------- Helpers & small types -----------------------------

        sealed class ActivationCoreInvoker
        {
            readonly UseCaseActivator _owner;
            readonly IdentifierSnapshot _idSnap;
            readonly IServiceRegistry _services;

            public ActivationCoreInvoker(UseCaseActivator owner, IBPUAIdentifier id, IServiceRegistry services)
            {
                _owner = owner;
                _services = services;
                _idSnap = new IdentifierSnapshot(id);
            }

            public Task<UseCaseActivationResult> Invoke()
            {
                return _owner.ActivateCoreAsync(_idSnap, _services);
            }
        }

        sealed class IdentifierSnapshot : IBPUAIdentifier
        {
            public IdentifierSnapshot(IBPUAIdentifier id)
            {
                DomainName = id.DomainName ?? string.Empty;
                UseCaseName = id.UseCaseName ?? string.Empty;
                StateName = id.StateName;
                TransitionName = id.TransitionName;
                Breadcrumbs = id.Breadcrumbs;
            }
            /// <summary>
            /// Gets or sets application layer name
            /// </summary>
            public string? ApplicationLayerName
            {
                get; set;
            }

            public string DomainName
            {
                get; set;
            }
            public string UseCaseName
            {
                get; set;
            }
            public string? StateName
            {
                get; set;
            }
            public string? TransitionName
            {
                get; set;
            }
            public string? Breadcrumbs
            {
                get; set;
            }
        }

        static string ResolveFolder(string pluginRoot, IBPUAIdentifier id)
        {
            string? subpath = !string.IsNullOrEmpty(id.Breadcrumbs) ? id.Breadcrumbs : id.UseCaseName;
            if (string.IsNullOrEmpty(subpath))
                return string.Empty;

            string normalized = subpath.Replace('\\', '/').Trim();
            if (normalized.StartsWith("/"))
                normalized = normalized.Substring(1);

            return Path.Combine(pluginRoot, normalized);
        }

        static string ComputeDefaultRoute(IBPUAIdentifier id)
        {
            string name;
            if (!string.IsNullOrEmpty(id.UseCaseName) &&
                !id.UseCaseName.Equals("Application", StringComparison.OrdinalIgnoreCase))
            {
                name = id.UseCaseName;
            }
            else
            {
                name = Breadcrumbs.GetBreadcrumsLastSegment(id.Breadcrumbs);
            }
            return "/u/" + name.ToLowerInvariant();
        }

        static string NormalizeActivationKey(IBPUAIdentifier id)
        {
            string domain = id.DomainName ?? string.Empty;
            string useCase = id.UseCaseName;

            if (string.IsNullOrEmpty(useCase) ||
                useCase.Equals("Application", StringComparison.OrdinalIgnoreCase))
            {
                string leaf = Breadcrumbs.GetBreadcrumsLastSegment(id.Breadcrumbs);
                useCase = string.IsNullOrEmpty(leaf) ? "application" : leaf;
            }

            string combined = string.IsNullOrEmpty(domain) ? useCase : domain + "." + useCase;
            return combined.Trim().ToLowerInvariant();
        }

        static string BuildStampKey(string normalizedKey) => "Activation:" + normalizedKey;

        static UseCaseActivationResult Failure(string message)
        {
            var r = new UseCaseActivationResult { Succeeded = false, NoAdditionalAssembliesWereLoaded = false };
            if (!string.IsNullOrEmpty(message))
                r.Errors.Add(message);
            return r;
        }

        // ----------------------------- Your existing properties -----------------------------

        public List<IBPUAAssemblyProcessor> ListOfAssemblyProcessors { get; set; } = default!;
        public List<Assembly> ListOfLoadedAssemblies { get; set; } = default!;
        public string PathToFolderWithDynamicAssemblies { get; set; } = default!;
    }
}
