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
        /// Loads dynamic assemblies from the supplied folder.
        /// When no explicit assembly file names are supplied, every DLL in the folder is considered.
        /// </summary>
        /// <param name="pathToFolderWithDynamicAssemblies">Path to folder with dynamic assemblies.</param>
        /// <param name="serviceRegistry">Service registry.</param>
        /// <param name="listOfLoadedAssemblies">List of loaded assemblies.</param>
        /// <param name="listOfAssemblyProcessors">List of assembly processors.</param>
        /// <returns>List of loaded dynamic assemblies.</returns>
        public List<Assembly> LoadDynamicAssemblies(
            string pathToFolderWithDynamicAssemblies,
            IServiceRegistry serviceRegistry,
            List<Assembly> listOfLoadedAssemblies,
            List<IBPUAAssemblyProcessor> listOfAssemblyProcessors)
        {
            return LoadDynamicAssemblies(
                pathToFolderWithDynamicAssemblies,
                serviceRegistry,
                listOfLoadedAssemblies,
                listOfAssemblyProcessors,
                null);
        }

        /// <summary>
        /// Loads only the requested dynamic assemblies from the supplied folder.
        /// When <paramref name="requestedAssemblyFileNames"/> is null or empty, every DLL in the folder is considered.
        /// When it contains file names, only those exact DLL files are considered for loading.
        /// </summary>
        /// <param name="pathToFolderWithDynamicAssemblies">Path to folder with dynamic assemblies.</param>
        /// <param name="serviceRegistry">Service registry.</param>
        /// <param name="listOfLoadedAssemblies">List of loaded assemblies.</param>
        /// <param name="listOfAssemblyProcessors">List of assembly processors.</param>
        /// <param name="requestedAssemblyFileNames">Optional exact assembly file names to load.</param>
        /// <returns>List of loaded dynamic assemblies.</returns>
        public List<Assembly> LoadDynamicAssemblies(
            string pathToFolderWithDynamicAssemblies,
            IServiceRegistry serviceRegistry,
            List<Assembly> listOfLoadedAssemblies,
            List<IBPUAAssemblyProcessor> listOfAssemblyProcessors,
            IList<string>? requestedAssemblyFileNames)
        {
            InitializeComponent(
                pathToFolderWithDynamicAssemblies,
                serviceRegistry,
                listOfLoadedAssemblies,
                listOfAssemblyProcessors,
                requestedAssemblyFileNames);

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
