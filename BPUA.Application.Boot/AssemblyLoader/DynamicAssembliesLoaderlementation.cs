using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using BPUA.Application.Contracts;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides dynamic assemblies loader functionality
    /// </summary>
    public partial class DynamicAssembliesLoader
    {
        #region Methods
        /// <summary>
        /// Adds static assembly processor to list of assembly processors
        /// </summary>
        void AddStaticAssemblyProcessorToListOfAssemblyProcessors()
        {
            bool exists = false;
            for (int i = 0; i < ListOfAssemblyProcessors.Count; i++)
            {
                if (ListOfAssemblyProcessors[i] is BpuaServiceAssemblyProcessor)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                ListOfAssemblyProcessors.Add(new BpuaServiceAssemblyProcessor());
            }
        }


        /// <summary>
        /// Adds the currently loaded assembly to the list of loaded assemblies when it is not already present.
        /// </summary>
        void AddLoadedAssemblyIfMissing()
        {
            if (LoadedAssembly == null)
            {
                return;
            }

            for (int i = 0; i < ListOfLoadedAssemblies.Count; i++)
            {
                Assembly assembly = ListOfLoadedAssemblies[i];
                if (string.Equals(assembly.FullName, LoadedAssembly.FullName, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            ListOfLoadedAssemblies.Add(LoadedAssembly);
        }

        /// <summary>
        /// Applies all configured assembly processors to the currently loaded assembly.
        /// </summary>
        void ProcessLoadedAssembly()
        {
            if (LoadedAssembly == null)
            {
                return;
            }

            for (int i = 0; i < ListOfAssemblyProcessors.Count; i++)
            {
                IBpuaAssemblyProcessor bpuaAssemblyProcessor = ListOfAssemblyProcessors[i];
                bpuaAssemblyProcessor.Process(LoadedAssembly, ServiceRegistry);
            }
        }

        /// <summary>
        /// Determines whether loaded assembly has LoadBPUAAssembly attribute
        /// </summary>
        /// <returns>Flag indicating whether loaded assembly has LoadBPUAAssembly attribute</returns>
        bool HasLoadBPUAAssemblyAttribute()
        {
            bool hasLoadBPUAAssemblyAttribute = LoadedAssembly!.IsDefined(typeof(LoadBPUAAssemblyAttribute), inherit: false);
            return hasLoadBPUAAssemblyAttribute;
        }

        /// <summary>
        /// Determines whether loaded assembly has ProvideBPUAProcessors attribute
        /// </summary>
        /// <returns>Flag indicating whether loaded assembly has ProvideBPUAProcessors attribute</returns>
        bool HasProvideBPUAProcessorsAttribute()
        {
            bool hasProvideBPUAProcessorsAttribute = LoadedAssembly!.IsDefined(typeof(ProvideBPUAProcessorsAttribute), inherit: false);
            return hasProvideBPUAProcessorsAttribute;
        }

        /// <summary>
        /// Initializes component
        /// </summary>
        /// <param name="pathToFolderWithDynamicAssemblies">Path to folder with dynamic assemblies</param>
        /// <param name="serviceRegistry">Service registry</param>
        /// <param name="listOfLoadedAssemblies">List of loaded assemblies</param>
        /// <param name="listOfAssemblyProcessors">List of assembly processors</param>
        void InitializeComponent(string pathToFolderWithDynamicAssemblies, IServiceRegistry serviceRegistry, List<Assembly> listOfLoadedAssemblies, List<IBpuaAssemblyProcessor> listOfAssemblyProcessors)
        {
            PathToFolderWithDynamicAssemblies = pathToFolderWithDynamicAssemblies;
            ServiceRegistry = serviceRegistry;
            ListOfLoadedAssemblies = listOfLoadedAssemblies;
            ListOfAssemblyProcessors = listOfAssemblyProcessors;
        }

        /// <summary>
        /// Loads dynamic assemblies
        /// </summary>
        void LoadDynamicAssemblies()
        {
            if (!Directory.Exists(PathToFolderWithDynamicAssemblies))
            {
                Console.WriteLine($"[AssemblyLoader] Folder not found: {PathToFolderWithDynamicAssemblies}");
                return;
            }

            string[] pathsToDynamicAssemblies = Directory.GetFiles(PathToFolderWithDynamicAssemblies, "*.dll");
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
                        AddLoadedAssemblyIfMissing();
                    }
                }
            }
        }

        /// <summary>
        /// Loads assembly processors
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
                // Partial load — use the successfully loaded types
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

                if (!typeof(IBpuaAssemblyProcessor).IsAssignableFrom(type))
                {
                    continue;
                }

                IBpuaAssemblyProcessor? bpuaAssemblyProcessor = Activator.CreateInstance(type) as IBpuaAssemblyProcessor;
                if (bpuaAssemblyProcessor != null)
                {
                    ListOfAssemblyProcessors.Add(bpuaAssemblyProcessor);
                    Console.WriteLine($"[AssemblyLoader] Loaded assembly processor: {type.FullName}");
                }
            }
        }

        /// <summary>
        /// Releases resources
        /// </summary>
        void ReleaseResources()
        {
            LoadedAssembly = null;
            PathToDynamicAssembly = default!;
            PathToFolderWithDynamicAssemblies = default!;
            ServiceRegistry = default!;
            ListOfLoadedAssemblies = default!;
            ListOfAssemblyProcessors = default!;
        }

        /// <summary>
        /// Tries to load assembly
        /// </summary>
        void TryToLoadAssembly()
        {
            try
            {
                LoadedAssembly = Assembly.LoadFrom(PathToDynamicAssembly);
            }
            catch (BadImageFormatException)
            {
                // Not a valid .NET assembly — ignore
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AssemblyLoader] Failed to load {PathToDynamicAssembly}: {ex.GetType().Name} - {ex.Message}");
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets list of assembly processors
        /// </summary>
        List<IBpuaAssemblyProcessor> ListOfAssemblyProcessors
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets loaded assembly
        /// </summary>
        Assembly? LoadedAssembly
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets list of loaded assemblies
        /// </summary>
        List<Assembly> ListOfLoadedAssemblies
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets path to dynamic assembly
        /// </summary>
        string PathToDynamicAssembly
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets path to folder with dynamic assemblies
        /// </summary>
        string PathToFolderWithDynamicAssemblies
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets service registry
        /// </summary>
        IServiceRegistry ServiceRegistry
        {
            get; set;
        } = default!;
        #endregion
    }
}
