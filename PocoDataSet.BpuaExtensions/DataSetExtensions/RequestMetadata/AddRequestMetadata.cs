using BPUA.Application.Contracts;
using BPUA.Core;

using PocoDataSet.Extensions;
using PocoDataSet.IData;

namespace PocoDataSet.BpuaExtensions
{
    /// <summary>
    /// Contains data set extension methods
    /// </summary>
    public static partial class DataSetExtensions
    {
        #region Public Methods
        /// <summary>
        /// Adds request metadata
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">Transition name</param>
        /// <param name="breadcrumbs">Breadcrumbs data</param>
        /// <returns>Added request metadata</returns>
        public static IRequestMetadata AddRequestMetadata(this IDataSet? dataSet, string? domainName, string? useCaseName, string? applicationLayerName, string? stateName, string? transitionName, string? breadcrumbs)
        {
            if (dataSet == null)
            {
                return default!;
            }

            IDataTable requestMetadataDataTable = dataSet.GetRequestMetadataDataTable();
            IDataRow requestMetadataDataRow = requestMetadataDataTable.AddNewRow();

            IRequestMetadata? requestMetadata = requestMetadataDataRow.AsInterface<IRequestMetadata>();
            requestMetadata.DomainName = domainName;
            requestMetadata.UseCaseName = useCaseName;  
            requestMetadata.ApplicationLayerName = applicationLayerName;
            requestMetadata.StateName = stateName;
            requestMetadata.TransitionName = transitionName;
            requestMetadata.Breadcrumbs = breadcrumbs;

            return requestMetadata;
        }
        #endregion
    }
}
