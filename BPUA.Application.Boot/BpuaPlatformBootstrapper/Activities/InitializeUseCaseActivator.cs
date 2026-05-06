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
        /// Initializes use case activator
        /// </summary>
        void InitializeUseCaseActivator()
        {
            UseCaseActivator useCaseActivator = new UseCaseActivator();
            useCaseActivator.ListOfAssemblyProcessors = ListOfAssemblyProcessors;
            useCaseActivator.PathToFolderWithDynamicAssemblies = PathToFolderWithDynamicAssemblies;

            IServiceRegistry serviceRegistry = BpuaApplication.GetInstance().ServiceRegistry;
            serviceRegistry.RegisterObject(typeof(IUseCaseActivator).Name, useCaseActivator);
        }
        #endregion
    }
}
