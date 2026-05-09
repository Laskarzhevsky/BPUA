namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines request names
    /// </summary>
    public class RequestNames
    {
        /// <summary>
        /// Defines any request name
        /// </summary>
        public static string ANY = "*";

        /// <summary>
        /// Initializing application request name
        /// </summary>
        public static string INITIALIZING_APPLICATION = "InitializingApplication";

        /// <summary>
        /// Send request to application next layer
        /// </summary>
        public static string SEND_REQUEST_TO_APPLICATION_NEXT_LAYER = "SendRequestToApplicationNextLayer";

        /// <summary>
        /// Send request to next handler
        /// </summary>
        public static string SEND_REQUEST_TO_NEXT_HANDLER = "SendRequestToNextHandler";
    }
}
