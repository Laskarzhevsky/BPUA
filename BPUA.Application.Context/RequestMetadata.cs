using BPUA.Application.Contracts;
using BPUA.Core;

namespace BPUA.Application.Context
{
    /// <summary>
    /// Defines request metadata data trasfer objet functionality
    /// </summary>
    public class RequestMetadata : BPUAIdentifier, IRequestMetadata
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets application layer full name
        /// </summary>
        public string? ApplicationLayerFullName
        {
            get;
        }

        /// <summary>
        /// Gets or sets flag indicating whether request handled
        /// </summary>
        public bool RequestHandled
        {
            get; set;
        }

        /// <summary>
        /// Gets full state name
        /// </summary>
        public string? StateFullName
        {
            get;
        }

        /// <summary>
        /// Gets or sets flag indicating whether statistics needs to be traced
        /// </summary>
        public bool TraceStatistics
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets trace subject
        /// </summary>
        public string? TraceSubject
        {
            get;
            set;
        }
        #endregion
    }
}
