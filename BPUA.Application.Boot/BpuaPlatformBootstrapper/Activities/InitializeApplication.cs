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
        /// Initializes application
        /// </summary>
        void InitializeApplication()
        {
            BpuaApplication.GetInstance().Initialize(ApplicationConfiguration, PathToFolderWithDynamicAssemblies);
        }
        #endregion
    }
}
