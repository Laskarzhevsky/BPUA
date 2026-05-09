using System.Threading.Tasks;

namespace BPUA.InfrastructureServer.DPL
{
    /// <summary>
    /// RegisteringHost service handler
    /// </summary>
    public partial class RegisteringHostTransitionHandler
    {
        #region Private Methods
        /// <summary>
        /// Saves merged application layers
        /// </summary>
        async Task SaveMergedApplicationLayers()
        {
            await RaiseServiceRequestEventAsync();
        }
        #endregion
    }
}
