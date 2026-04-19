using System;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Provides service request event arguments functionality
    /// </summary>
    public class ServiceRequestEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="eventSourceType">Event source type</param>
        /// <param name="eventName">Event name</param>
        /// <param name="eventArguments">Event arguments</param>
        public ServiceRequestEventArgs(Type eventSourceType, string eventName, EventArgs eventArguments)
        {
            EventName = eventName;
            EventArguments = eventArguments;
            EventSourceType = eventSourceType;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets event arguments
        /// </summary>
        public EventArgs EventArguments
        {
            get; private set;
        }

        /// <summary>
        /// Gets event name
        /// </summary>
        public string EventName
        {
            get; private set;
        }

        /// <summary>
        /// Gets event source type
        /// </summary>
        public Type EventSourceType
        {
            get; private set;
        }
        #endregion
    }
}
