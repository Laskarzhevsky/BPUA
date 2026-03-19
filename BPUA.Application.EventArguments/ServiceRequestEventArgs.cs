using System;

namespace BPUA.Application.EventArguments
{
    /// <summary>
    /// Provides route transition context event arguments functionality
    /// </summary>
    public class ServiceRequestEventArgs : EventArgs
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets event name
        /// </summary>
        public string EventName
        {
            get; set;
        } = string.Empty;
        #endregion
    }
}
