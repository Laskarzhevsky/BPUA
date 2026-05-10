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
        /// <param name="eventArguments">Event arguments</param>
        public ServiceRequestEventArgs(EventArgs eventArguments)
        {
            EventArguments = eventArguments;
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
        #endregion
    }
}
