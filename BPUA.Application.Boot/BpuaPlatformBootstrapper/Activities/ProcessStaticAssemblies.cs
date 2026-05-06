using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides BPUA platform bootstrapper functionality
    /// </summary>
    public partial class BpuaPlatformBootstrapper
    {
        #region Methods
        /// <summary>
        /// Processes the already loaded static platform assemblies through all configured
        /// assembly processors so that built-in handlers and routers are registered during boot.
        /// Dynamic plugin assemblies are still loaded only on demand later by UseCaseActivator.
        /// </summary>
        void ProcessStaticAssemblies()
        {
            IServiceRegistry serviceRegistry = BpuaApplication.GetInstance().ServiceRegistry;

            for (int i = 0; i < ListOfLoadedAssemblies.Count; i++)
            {
                System.Reflection.Assembly loadedAssembly = ListOfLoadedAssemblies[i];

                for (int j = 0; j < ListOfAssemblyProcessors.Count; j++)
                {
                    IBpuaAssemblyProcessor assemblyProcessor = ListOfAssemblyProcessors[j];
                    assemblyProcessor.Process(loadedAssembly, serviceRegistry);
                }
            }
        }
        #endregion
    }
}
