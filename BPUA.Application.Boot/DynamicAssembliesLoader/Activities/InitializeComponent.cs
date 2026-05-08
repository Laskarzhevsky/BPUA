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
        #region Methods
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
        #endregion
    }
}
