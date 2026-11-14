using BPUA.Application.Orchestration;
using BPUA.Application.Contracts;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides BPUA platform bootstrapper functionality
    /// </summary>
    public partial class BpuaPlatformBootstrapper
    {
        #region Methods
        /// <summary>
        /// Initializes application
        /// </summary>
        void InitializeApplication()
        {
            BPUA.Application.Orchestration.BpuaApplication application = new BPUA.Application.Orchestration.BpuaApplication();
            application.Initialize(ApplicationConfiguration, PathToFolderWithDynamicAssemblies);
            BPUA.Application.Contracts.BpuaApplication.Initialize(application);
        }
        #endregion
    }
}
