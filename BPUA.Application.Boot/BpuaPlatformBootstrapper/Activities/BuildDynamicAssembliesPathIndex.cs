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
        /// Builds the dynamic assemblies path index
        /// </summary>
        void BuildDynamicAssembliesPathIndex()
        {
            IBpuaApplication application = BpuaApplication.GetInstance();
            DynamicAssemblyPathIndexBuilder.BuildAssemblyPathIndex(application.PathToFolderWithDynamicAssemblies, application.ServiceRegistry);
        }
        #endregion
    }
}
