using System.Threading.Tasks;

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
        /// Initializes hosted application layers
        /// </summary>
        async Task InitializeHostedApplicationLayers()
        {
            IBpuaApplication application = BpuaApplication.GetInstance();
            await application.InitializeHostedApplicationLayers();
        }
        #endregion
    }
}
