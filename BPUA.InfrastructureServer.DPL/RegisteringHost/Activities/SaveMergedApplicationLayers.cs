using System.Threading.Tasks;

namespace BPUA.InfrastructureServer.DPL
{
    /// <summary>
    /// RegisteringHost service handler
    /// </summary>
    public partial class RegisteringHostRequestHandler
    {
        #region Private Methods
        /// <summary>
        /// Saves merged application layers
        /// </summary>
        async Task SaveMergedApplicationLayers()
        {
            await RaiseServiceRequestEventAsync(BPUA.InfrastructureServer.Contracts.RequestNames.SAVE_MERGED_APPLICATION_LAYERS);
        }
        #endregion
    }
}
