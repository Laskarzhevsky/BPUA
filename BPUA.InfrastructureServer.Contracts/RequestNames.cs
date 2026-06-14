namespace BPUA.InfrastructureServer.Contracts
{
    /// <summary>
    /// Defines request names
    /// </summary>
    public class RequestNames : BPUA.Application.Contracts.RequestNames
    {
        /// <summary>
        /// RegisterHost request name
        /// </summary>
        public const string REGISTER_HOST = "RegisterHost";

        /// <summary>
        /// SearchApplicationLayersByFullNames request name
        /// </summary>
        public const string SEARCH_APPLICATION_LAYERS_BY_FULL_NAMES = "SearchApplicationLayersByFullNames";
    }
}
