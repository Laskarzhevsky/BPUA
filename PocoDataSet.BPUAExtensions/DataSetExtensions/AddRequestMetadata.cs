using BPUA.Application.Contracts;
using BPUA.Core;

using PocoDataSet.IData;

namespace PocoDataSet.BPUAExtensions
{
    /// <summary>
    /// Contains data set extensions methods
    /// </summary>
    public static partial class DataSetExtensions
    {
        #region Public Methods
        /// <summary>
        /// Adds request metadata
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <param name="bpuaIdentifier">BPUA identifier</param>
        public static void AddRequestMetadata(this IDataSet dataSet, IBPUAIdentifier bpuaIdentifier)
        {
            AddRequestMetadata(dataSet, bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName, bpuaIdentifier.StateName, bpuaIdentifier.TransitionName, bpuaIdentifier.Breadcrumbs);
        }

        /// <summary>
        /// Adds request metadata
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <param name="requestMetadata">Request metadata</param>
        public static void AddRequestMetadata(this IDataSet dataSet, IRequestMetadata requestMetadata)
        {
            AddRequestMetadata(dataSet, requestMetadata.DomainName, requestMetadata.UseCaseName, requestMetadata.ApplicationLayerName, requestMetadata.StateName, requestMetadata.TransitionName, requestMetadata.Breadcrumbs);
        }

        /// <summary>
        /// Adds request metadata
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">Transition name</param>
        /// <param name="breadcrumbs">Breadcrumbs string</param>
        public static void AddRequestMetadata(this IDataSet dataSet, string? domainName, string? useCaseName, string? applicationLayerName, string? stateName, string? transitionName, string? breadcrumbs = null)
        {
            IRequestMetadata requestMetadata = GetNewRequestMetadataAsInterface(dataSet);
            requestMetadata.DomainName = domainName;
            requestMetadata.UseCaseName = useCaseName;
            requestMetadata.ApplicationLayerName = applicationLayerName;
            requestMetadata.StateName = stateName;
            requestMetadata.TransitionName = transitionName;
            requestMetadata.Breadcrumbs = breadcrumbs;
        }
        #endregion
    }
}
