using BPUA.Application.Contracts;

using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides dynamic assemblies loader functionality
    /// </summary>
    public partial class DynamicAssembliesLoader
    {
        #region Public Methods
        /// <summary>
        /// Loads transition companion assemblies from a use case folder and applies all configured assembly processors to them.
        /// Only assemblies following the *.Transitions.dll naming convention are considered. This prevents unrelated layer
        /// assemblies such as DAL or BL from being loaded while still allowing the transition assembly marker attribute to be verified.
        /// </summary>
        /// <param name="pathToUseCaseFolder">Path to the use case folder.</param>
        /// <param name="serviceRegistry">Service registry.</param>
        /// <param name="listOfLoadedAssemblies">List of loaded assemblies.</param>
        /// <param name="listOfAssemblyProcessors">List of assembly processors.</param>
        /// <returns>List of loaded transition assemblies.</returns>
        public List<Assembly> LoadDynamicTransitionAssembliesFromFolder(string pathToUseCaseFolder, IServiceRegistry serviceRegistry, List<Assembly> listOfLoadedAssemblies, List<IBpuaAssemblyProcessor> listOfAssemblyProcessors)
        {
            List<Assembly> loadedTransitionAssemblies = new List<Assembly>();

            InitializeComponent(pathToUseCaseFolder, serviceRegistry, listOfLoadedAssemblies, listOfAssemblyProcessors);
            AddStaticAssemblyProcessorsToListOfAssemblyProcessors();

            if (!Directory.Exists(pathToUseCaseFolder))
            {
                ReleaseResources();
                return loadedTransitionAssemblies;
            }

            string[] pathsToDynamicAssemblies = Directory.GetFiles(pathToUseCaseFolder, "*.Transitions.dll", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < pathsToDynamicAssemblies.Length; i++)
            {
                LoadedAssembly = null;
                PathToDynamicAssembly = pathsToDynamicAssemblies[i];
                TryToLoadAssembly();
                if (LoadedAssembly == null)
                {
                    continue;
                }

                if (!HasLoadBPUAAssemblyAttribute())
                {
                    continue;
                }

                if (!LoadedAssembly.IsDefined(typeof(BPUA.Application.Contracts.RegisterAsBPUATransitionAssemblyAttribute), inherit: false))
                {
                    continue;
                }

                AddLoadedAssemblyIfMissing();
                ProcessLoadedAssembly();
                loadedTransitionAssemblies.Add(LoadedAssembly);
            }

            ReleaseResources();
            return loadedTransitionAssemblies;
        }
        #endregion
    }
}
