using BPUA.Application.Contracts;

namespace BPUA.Http.Drivers
{
    /// <summary>
    /// Provides host data validator class
    /// </summary>
    public class DnsRecordValidator
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="bpuaInfrastructureServerRecord">BPUA infrastructure server record</param>
        /// <param name="logValidationResult">Flag indicating whether validation result needs to be logged</param>
        /// <param name="messageHeader">Message header</param>
        public DnsRecordValidator(IBpuaInfrastructureServerRecord bpuaInfrastructureServerRecord, bool logValidationResult, string? messageHeader = null)
        {
            BpuaInfrastructureServerRecord = bpuaInfrastructureServerRecord;
            LogValidationResult = logValidationResult;
            MessageHeader = messageHeader;
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Handles request
        /// </summary>
        protected void HandleRequest()
        {
            ValidateHostApplicationLayerName();
            if (DnsRecordDataValid)
            {
                ValidateUrl();
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Logs message
        /// </summary>
        /// <param name="message">Message for logging</param>
        /// <param name="messageType">Message type</param>
        protected virtual void LogMessage(string message, MessageType messageType)
        {
            DataContainer.SetMessage(message, messageType, BpuaInfrastructureServerRecord.ApplicationLayerFullName, BpuaInfrastructureServerRecord.Url);
        }

        /// <summary>
        /// Validates host application layer name
        /// </summary>
        void ValidateHostApplicationLayerName()
        {
            if (string.IsNullOrEmpty(BpuaInfrastructureServerRecord.ApplicationLayerFullName))
            {
                if (LogValidationResult)
                {
                    LogMessage($"{MessageHeader}DNS data does not have required ApplicationLayerName entry", MessageType.Error);
                }

                DnsRecordDataValid = false;
            }
        }

        /// <summary>
        /// Validates URL
        /// </summary>
        void ValidateUrl()
        {
            if (string.IsNullOrEmpty(BpuaInfrastructureServerRecord.Url))
            {
                if (LogValidationResult)
                {
                    LogMessage($"{MessageHeader}DNS data file does not have required URL entry for " + BpuaInfrastructureServerRecord.ApplicationLayerFullName, MessageType.Warning);
                }

                DnsRecordDataValid = false;
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets flag indicating whether host data valid
        /// </summary>
        public bool DnsRecordDataValid
        {
            get; set;
        } = true;
        #endregion

        #region Private Properties
        /// <summary>
        /// Gets or sets BPUA infrastructure server record
        /// </summary>
        IBpuaInfrastructureServerRecord BpuaInfrastructureServerRecord
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets flag indicating whether validation result needs to be logged
        /// </summary>
        bool LogValidationResult
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets message header
        /// </summary>
        string? MessageHeader
        {
            get; set;
        }
        #endregion
    }
}
