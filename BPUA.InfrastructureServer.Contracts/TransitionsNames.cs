namespace BPUA.InfrastructureServer.Contracts
{
    /// <summary>
    /// Defines transition names
    /// </summary>
    public class TransitionsNames : BPUA.Application.Contracts.TransitionsNames
    {
        /// <summary>
        /// RegisteringHost transition name
        /// </summary>
        public static string REGISTERING_HOST = "RegisteringHost";

        /// <summary>
        /// SearchingApplicationLayersByFullNames transition name
        /// </summary>
        public const string SEARCHING_APPLICATION_LAYERS_BY_FULL_NAMES = "SearchingApplicationLayersByFullNames";
    }
}
