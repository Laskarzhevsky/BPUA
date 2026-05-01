namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines table names
    /// </summary>
    public class TableNames
    {
        /// <summary>
        /// Hosted application layer
        /// </summary>
        public static string HOSTED_APPLICATION_LAYER = "HostedApplicationLayer";

        /// <summary>
        /// Infrastructure server table
        /// </summary>
        public static string INFRASTRUCTURE_SERVER = "InfrastructureServer";

        /// <summary>
        /// Host suffix
        /// </summary>
        public const string HOST_SUFFIX = "_Host";

        /// <summary>
        /// Message table
        /// </summary>
        public static string MESSAGE = "__Message";

        /// <summary>
        /// Request suffix
        /// </summary>
        public static string _REQUEST = "Request";

        /// <summary>
        /// Response suffix
        /// </summary>
        public static string _RESPONSE = "Response";

        /// <summary>
        /// Request metadata
        /// </summary>
        public static string REQUEST_METADATA = "__RequestMetadata";

        /// <summary>
        /// Search table
        /// </summary>
        public static string SEARCH = "Search";

        /// <summary>
        /// Transition handler
        /// </summary>
        public const string TRANSITION_HANDLER = "TransitionHandler";

        /// <summary>
        /// Transition metadata
        /// </summary>
        public static string TRANSITION_METADATA = "__TransitionMetadata";
    }
}
