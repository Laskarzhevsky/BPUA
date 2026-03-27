using System;
using System.Collections.Generic;
using System.Reflection;

using BPUA.Application.Contracts;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides dynamic assemblies loader functionality.
    /// </summary>
    public partial class DynamicAssembliesLoader
    {
        #region Public Methods
        /// <summary>
        /// Loads a single dynamic assembly and applies all configured assembly processors to it.
        /// </summary>
        /// <param name="pathToDynamicAssembly">Path to the dynamic assembly to load.</param>
        /// <param name="serviceRegistry">Service registry.</param>
        /// <param name="listOfLoadedAssemblies">List of loaded assemblies.</param>
        /// <param name="listOfAssemblyProcessors">List of assembly processors.</param>
        /// <returns>The loaded assembly, or null when the file could not be loaded or was not eligible.</returns>
        public Assembly? LoadDynamicAssembly(string pathToDynamicAssembly, IServiceRegistry serviceRegistry, List<Assembly> listOfLoadedAssemblies, List<IBPUAAssemblyProcessor> listOfAssemblyProcessors)
        {
            InitializeComponent(string.Empty, serviceRegistry, listOfLoadedAssemblies, listOfAssemblyProcessors);
            PathToDynamicAssembly = pathToDynamicAssembly;

            AddStaticAssemblyProcessorToListOfAssemblyProcessors();
            LoadedAssembly = null;
            TryToLoadAssembly();
            if (LoadedAssembly == null)
            {
                ReleaseResources();
                return null;
            }

            if (!HasLoadBPUAAssemblyAttribute())
            {
                ReleaseResources();
                return null;
            }

            if (HasProvideBPUAProcessorsAttribute())
            {
                LoadAssemblyProcessors();
            }
            else
            {
                AddLoadedAssemblyIfMissing();
            }

            ProcessLoadedAssembly();

            Assembly loadedAssembly = LoadedAssembly;
            ReleaseResources();
            return loadedAssembly;
        }

        /// <summary>
        /// Loads dynamic assemblies from a folder.
        /// This method is retained for compatibility but should not be used by the current boot pipeline.
        /// </summary>
        /// <param name="pathToFolderWithDynamicAssemblies">Path to folder with dynamic assemblies.</param>
        /// <param name="serviceRegistry">Service registry.</param>
        /// <param name="listOfLoadedAssemblies">List of loaded assemblies.</param>
        /// <param name="listOfAssemblyProcessors">List of assembly processors.</param>
        /// <returns>List of loaded dynamic assemblies.</returns>
        [Obsolete("Use LoadDynamicAssembly for on-demand loading by identifier.")]
        public List<Assembly> LoadDynamicAssemblies(string pathToFolderWithDynamicAssemblies, IServiceRegistry serviceRegistry, List<Assembly> listOfLoadedAssemblies, List<IBPUAAssemblyProcessor> listOfAssemblyProcessors)
        {
            InitializeComponent(pathToFolderWithDynamicAssemblies, serviceRegistry, listOfLoadedAssemblies, listOfAssemblyProcessors);

            LoadDynamicAssemblies();
            AddStaticAssemblyProcessorToListOfAssemblyProcessors();
            foreach (Assembly assembly in listOfLoadedAssemblies)
            {
                foreach (IBPUAAssemblyProcessor bpuaAssemblyProcessor in ListOfAssemblyProcessors)
                {
                    bpuaAssemblyProcessor.Process(assembly, serviceRegistry);
                }
            }

            ReleaseResources();
            return listOfLoadedAssemblies;
        }
        #endregion
    }
}
