using System.Collections.Generic;
using System.Reflection;

using BPUA.Application.Contracts;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides dynamic assemblies loader functionality
    /// </summary>
    public partial class DynamicAssembliesLoader
    {
        #region Public Methods
        /// <summary>
        /// Loads dynamic assemblies
        /// </summary>
        /// <param name="pathToFolderWithDynamicAssemblies">Path to folder with dynamic assemblies</param>
        /// <param name="serviceRegistry">Service registry</param>
        /// <param name="listOfLoadedAssemblies">List of loaded assemblies</param>
        /// <param name="listOfAssemblyProcessors">List of assembly processors</param>
        /// <returns>List of loaded dynamic assemblies</returns>
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
