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
        /// Searches application layers by full names
        /// </summary>
        async Task SearchApplicationLayersByFullNames()
        {
            await RaiseServiceRequestEventAsync();
        }
        #endregion
    }
}
