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
            BpuaApplication application = new BpuaApplication();
            application.Initialize(ApplicationConfiguration, PathToFolderWithDynamicAssemblies);
            ManagementSystem.Initialize(application);
        }
        #endregion
    }
}
