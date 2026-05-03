using System;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines message functionality
    /// </summary>
    public interface IMessage
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets application layer name
        /// </summary>
        string? ApplicationLayerName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets application layer URL
        /// </summary>
        string? ApplicationLayerUrl
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets exception
        /// </summary>
        Exception? Exception
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets message code
        /// </summary>
        string MessageCode
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets message text
        /// </summary>
        string? MessageText
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets message type
        /// </summary>
        MessageType MessageType
        {
            get; set;
        }
        #endregion
    }
}
