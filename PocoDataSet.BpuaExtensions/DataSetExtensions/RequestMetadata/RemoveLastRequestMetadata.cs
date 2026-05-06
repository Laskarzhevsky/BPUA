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
        public static void RemoveLastRequestMetadata(this IDataSet? dataSet)
        {
if (dataSet == null)
{
    return;
}

IDataTable requestMetadataDataTable = dataSet.GetRequestMetadataDataTable();
if (requestMetadataDataTable.Rows.Count > 0)
{
    IRequestMetadata? currentRequestMetadata = dataSet.GetCurrentRequestMetadata();

    // Copy current request metadata
    BpuIdentifier bpuIdentifier = new BpuIdentifier();
    bpuIdentifier.ApplicationLayerName = currentRequestMetadata!.ApplicationLayerName;
    bpuIdentifier.DomainName = currentRequestMetadata.DomainName;
    bpuIdentifier.StateName = currentRequestMetadata.StateName;
    bpuIdentifier.TransitionName = currentRequestMetadata.TransitionName;
    bpuIdentifier.UseCaseName = currentRequestMetadata.UseCaseName;

    dataSet.RemoveRow(BPUA.Application.Contracts.TableNames.REQUEST_METADATA, requestMetadataDataTable.Rows.Count - 1);

    // Propagate current use case matadata to previous for BL layer
    if (bpuIdentifier.ApplicationLayerName == ApplicationLayersNames.BL)
    {
        currentRequestMetadata = dataSet!.GetCurrentRequestMetadata();
        if (currentRequestMetadata!.DomainName != bpuIdentifier.DomainName)
        {
currentRequestMetadata.DomainName = bpuIdentifier.DomainName;
        }

        if (currentRequestMetadata.StateName != bpuIdentifier.StateName)
        {
currentRequestMetadata.StateName = bpuIdentifier.StateName;
        }

        if (currentRequestMetadata.TransitionName != bpuIdentifier.TransitionName)
        {
currentRequestMetadata.TransitionName = bpuIdentifier.TransitionName;
        }

        if (currentRequestMetadata.UseCaseName != bpuIdentifier.UseCaseName)
        {
currentRequestMetadata.UseCaseName = bpuIdentifier.UseCaseName;
        }
    }
}
        }
        #endregion
    }
}
