using BPUA.Core;

using PocoDataSet.IData;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines request metadata data trasfer objet functionality
    /// </summary>
    public interface IRequestMetadata : IBPUAIdentifier
    {
        #region Properties
        /// <summary>
        /// Gets or sets application layer full name
        /// </summary>
        string? ApplicationLayerFullName
        {
            get;
        }

        /// <summary>
        /// Gets or sets flag indicating whether request handled
        /// </summary>
        bool RequestHandled
        {
            get; set;
        }

        /// <summary>
        /// Gets full state name
        /// </summary>
        string? StateFullName
        {
            get;
        }

        /// <summary>
        /// Gets or sets flag indicating whether statistics needs to be traced
        /// </summary>
        bool TraceStatistics
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets trace subject
        /// </summary>
        string? TraceSubject
        {
            get;
            set;
        }
        #endregion
    }
}
