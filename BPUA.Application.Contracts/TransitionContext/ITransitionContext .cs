using BPUA.Core;

using PocoDataSet.IData;

using System.Collections.Generic;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines transition context
    /// </summary>
    public interface ITransitionContext
    {
        #region Methods
        /// <summary>
        /// Adds request metadata
        /// </summary>
        /// <param name="bpuaIdentifier">BPUA identifier</param>
        void AddRequestMetadata(IBPUAIdentifier bpuaIdentifier);

        /// <summary>
        /// Adds request metadata
        /// </summary>
        /// <param name="requestMetadata">Request metadata</param>
        void AddRequestMetadata(IRequestMetadata requestMetadata);

        /// <summary>
        /// Adds request metadata
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">Transition name</param>
        /// <param name="breadcrumbs">Breadcrumbs string</param>
        void AddRequestMetadata(string? domainName, string? useCaseName, string? applicationLayerName, string? stateName, string? transitionName, string? breadcrumbs = null);

        /// <summary>
        /// Removes current request metadata
        /// </summary>
        void RemoveCurrentRequestMetadata();
        #endregion

        #region Properties
        /// <summary>
        /// Gets BPUA identifier
        /// </summary>
        IBPUAIdentifier BPUAIdentifier
        {
            get;
        }

        /// <summary>
        /// Gets data set
        /// </summary>
        IDataSet DataSet
        {
            get;
        }

        /// <summary>
        /// Gets request metadata
        /// </summary>
        IRequestMetadata RequestMetadata
        {
            get;
        }

        /// <summary>
        /// Gets transition metadata
        /// </summary>
        IReadOnlyList<ITransitionMetadata> TransitionsMetadata
        {
            get;
        }
        #endregion
    }
}
