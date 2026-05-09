using BPUA.Application.Contracts;

namespace BPUA.Application
{
    /// <summary>
    /// Provides transition selection key functionality.
    /// </summary>
    public class RequestRouteKey : ITransitionKey
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="requestorTypeFullName">Requestor type full name</param>
        /// <param name="eventName">Event name</param>
        /// <param name="requestedTransitionName">The name of the requested transition, if any.</param>
        public RequestRouteKey(string requestorTypeFullName, string eventName, string? requestedTransitionName = null)
        {
            RequestorTypeFullName = requestorTypeFullName;
            EventName = eventName;
            RequestedTransitionName = requestedTransitionName;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets event name
        /// ITransitionDefinitionKey interface implementation
        /// </summary>
        public string EventName
        {
            get; private set;
        }

        /// <summary>
        /// Gets the name of the requested transition, if any.
        /// ITransitionDefinitionKey interface implementation
        /// </summary>
        /// <remarks>This property may return null if no transition has been requested.</remarks>
        public string? RequestedTransitionName
        {
            get; private set;
        }

        /// <summary>
        /// Gets requestor type full name
        /// ITransitionDefinitionKey interface implementation
        /// </summary>
        public string RequestorTypeFullName
        {
            get; private set;
        }
        #endregion
    }
}
