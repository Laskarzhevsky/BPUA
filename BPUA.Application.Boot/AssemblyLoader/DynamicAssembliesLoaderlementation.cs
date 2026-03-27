using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using BPUA.Application.Contracts;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides dynamic assemblies loader functionality.
    /// </summary>
    public partial class DynamicAssembliesLoader
    {
        #region Methods
        /// <summary>
        /// Adds the built-in BPUA service assembly processor when it is not already present.
        /// </summary>
        void AddStaticAssemblyProcessorToListOfAssemblyProcessors()
        {
            bool exists = false;
            for (int i = 0; i < ListOfAssemblyProcessors.Count; i++)
            {
                if (ListOfAssemblyProcessors[i] is BPUAServiceAssemblyProcessor)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                ListOfAssemblyProcessors.Add(new BPUAServiceAssemblyProcessor());
            }
        }

        /// <summary>
        /// Determines whether the currently loaded assembly has the LoadBPUAAssembly attribute.
        /// </summary>
        /// <returns>True when the assembly is marked for BPUA dynamic loading.</returns>
        bool HasLoadBPUAAssemblyAttribute()
        {
            bool hasLoadBPUAAssemblyAttribute = LoadedAssembly!.IsDefined(typeof(LoadBPUAAssemblyAttribute), inherit: false);
            return hasLoadBPUAAssemblyAttribute;
        }

        /// <summary>
        /// Determines whether the currently loaded assembly provides assembly processors.
        /// </summary>
        /// <returns>True when the assembly is marked with ProvideBPUAProcessorsAttribute.</returns>
        bool HasProvideBPUAProcessorsAttribute()
        {
            bool hasProvideBPUAProcessorsAttribute = LoadedAssembly!.IsDefined(typeof(ProvideBPUAProcessorsAttribute), inherit: false);
            return hasProvideBPUAProcessorsAttribute;
        }

        /// <summary>
        /// Initializes the loader for one loading run.
        /// </summary>
        /// <param name="pathToFolderWithDynamicAssemblies">Path to folder with dynamic assemblies.</param>
        /// <param name="serviceRegistry">Service registry.</param>
        /// <param name="listOfLoadedAssemblies">List of loaded assemblies.</param>
        /// <param name="listOfAssemblyProcessors">List of assembly processors.</param>
        /// <param name="requestedAssemblyFileNames">Optional exact assembly file names to load.</param>
        void InitializeComponent(
            string pathToFolderWithDynamicAssemblies,
            IServiceRegistry serviceRegistry,
            List<Assembly> listOfLoadedAssemblies,
            List<IBPUAAssemblyProcessor> listOfAssemblyProcessors,
            IList<string>? requestedAssemblyFileNames)
        {
            PathToFolderWithDynamicAssemblies = pathToFolderWithDynamicAssemblies;
            ServiceRegistry = serviceRegistry;
            ListOfLoadedAssemblies = listOfLoadedAssemblies;
            ListOfAssemblyProcessors = listOfAssemblyProcessors;
            RequestedAssemblyFileNames = requestedAssemblyFileNames;
        }

        /// <summary>
        /// Loads dynamic assemblies from the configured folder.
        /// Only the explicitly requested file names are considered when the caller supplied them.
        /// Otherwise all DLL files in the folder are considered.
        /// </summary>
        void LoadDynamicAssemblies()
        {
            if (!Directory.Exists(PathToFolderWithDynamicAssemblies))
            {
                Console.WriteLine($"[AssemblyLoader] Folder not found: {PathToFolderWithDynamicAssemblies}");
                return;
            }

            string[] pathsToDynamicAssemblies = ResolveAssemblyPathsToLoad();
            for (int i = 0; i < pathsToDynamicAssemblies.Length; i++)
            {
                LoadedAssembly = null;
                PathToDynamicAssembly = pathsToDynamicAssemblies[i];
                TryToLoadAssembly();
                if (LoadedAssembly == null)
                {
                    continue;
                }

                if (HasLoadBPUAAssemblyAttribute())
                {
                    Console.WriteLine($"[AssemblyLoader] Loaded assembly {PathToDynamicAssembly}");
                    if (HasProvideBPUAProcessorsAttribute())
                    {
                        LoadAssemblyProcessors();
                    }
                    else
                    {
                        ListOfLoadedAssemblies.Add(LoadedAssembly);
                    }
                }
            }
        }

        /// <summary>
        /// Resolves the physical assembly paths that should be loaded for the current run.
        /// </summary>
        /// <returns>Array of physical assembly paths to consider for loading.</returns>
        string[] ResolveAssemblyPathsToLoad()
        {
            if (RequestedAssemblyFileNames == null || RequestedAssemblyFileNames.Count == 0)
            {
                return Directory.GetFiles(PathToFolderWithDynamicAssemblies, "*.dll");
            }

            List<string> resolvedPaths = new List<string>();
            for (int i = 0; i < RequestedAssemblyFileNames.Count; i++)
            {
                string requestedAssemblyFileName = RequestedAssemblyFileNames[i];
                if (string.IsNullOrWhiteSpace(requestedAssemblyFileName))
                {
                    continue;
                }

                string candidatePath = Path.Combine(PathToFolderWithDynamicAssemblies, requestedAssemblyFileName);
                if (File.Exists(candidatePath))
                {
                    resolvedPaths.Add(candidatePath);
                }
                else
                {
                    Console.WriteLine($"[AssemblyLoader] Requested assembly was not found: {candidatePath}");
                }
            }

            return resolvedPaths.ToArray();
        }

        /// <summary>
        /// Loads assembly processors from the currently loaded assembly.
        /// </summary>
        void LoadAssemblyProcessors()
        {
            Type?[] types;
            try
            {
                types = LoadedAssembly!.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types ?? Array.Empty<Type>();
            }

            for (int i = 0; i < types.Length; i++)
            {
                Type? type = types[i];
                if (type == null)
                {
                    continue;
                }

                if (!type.IsClass || type.IsAbstract)
                {
                    continue;
                }

                if (!typeof(IBPUAAssemblyProcessor).IsAssignableFrom(type))
                {
                    continue;
                }

                IBPUAAssemblyProcessor? bpuaAssemblyProcessor = Activator.CreateInstance(type) as IBPUAAssemblyProcessor;
                if (bpuaAssemblyProcessor != null)
                {
                    ListOfAssemblyProcessors.Add(bpuaAssemblyProcessor);
                    Console.WriteLine($"[AssemblyLoader] Loaded assembly processor: {type.FullName}");
                }
            }
        }

        /// <summary>
        /// Releases loader state captured for the current run.
        /// </summary>
        void ReleaseResources()
        {
            LoadedAssembly = null;
            ServiceRegistry = default!;
            ListOfLoadedAssemblies = default!;
            RequestedAssemblyFileNames = default!;
        }

        /// <summary>
        /// Tries to load the current assembly file.
        /// </summary>
        void TryToLoadAssembly()
        {
            try
            {
                LoadedAssembly = Assembly.LoadFrom(PathToDynamicAssembly);
            }
            catch (BadImageFormatException)
            {
                // Not a valid .NET assembly — ignore.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AssemblyLoader] Failed to load {PathToDynamicAssembly}: {ex.GetType().Name} - {ex.Message}");
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets list of assembly processors.
        /// </summary>
        List<IBPUAAssemblyProcessor> ListOfAssemblyProcessors
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets loaded assembly.
        /// </summary>
        Assembly? LoadedAssembly
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets list of loaded assemblies.
        /// </summary>
        List<Assembly> ListOfLoadedAssemblies
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets path to dynamic assembly.
        /// </summary>
        string PathToDynamicAssembly
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets path to folder with dynamic assemblies.
        /// </summary>
        string PathToFolderWithDynamicAssemblies
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets service registry.
        /// </summary>
        IServiceRegistry ServiceRegistry
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets the optional list of exact assembly file names that should be loaded.
        /// </summary>
        IList<string>? RequestedAssemblyFileNames
        {
            get; set;
        }
        #endregion
    }
}
