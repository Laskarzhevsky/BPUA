using BPUA.InfrastructureServer.Contracts;

using System.Threading.Tasks;

namespace BPUA.InfrastructureServer.DPL
{
    /// <summary>
    /// SearchApplicationLayersByFullNames service handler
    /// </summary>
    public partial class SearchApplicationLayersByFullNamesRequestHandler
    {
        #region Private Methods
        /// <summary>
        /// Searches application layers by full names
        /// </summary>
        async Task SearchApplicationLayersByFullNames()
        {
            await RaiseServiceRequestEventAsync(RequestNames.SEARCH_APPLICATION_LAYERS_BY_FULL_NAMES);
        }
        #endregion
    }
}
