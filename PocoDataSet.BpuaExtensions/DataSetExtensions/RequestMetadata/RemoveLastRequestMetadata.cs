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
    BPUAIdentifier bpuaIdentifier = new BPUAIdentifier();
    bpuaIdentifier.ApplicationLayerName = currentRequestMetadata!.ApplicationLayerName;
    bpuaIdentifier.DomainName = currentRequestMetadata.DomainName;
    bpuaIdentifier.StateName = currentRequestMetadata.StateName;
    bpuaIdentifier.TransitionName = currentRequestMetadata.TransitionName;
    bpuaIdentifier.UseCaseName = currentRequestMetadata.UseCaseName;

    dataSet.RemoveRow(BPUA.Application.Contracts.TableNames.REQUEST_METADATA, requestMetadataDataTable.Rows.Count - 1);

    // Propagate current use case matadata to previous for BL layer
    if (bpuaIdentifier.ApplicationLayerName == ApplicationLayersNames.BL)
    {
        currentRequestMetadata = dataSet!.GetCurrentRequestMetadata();
        if (currentRequestMetadata!.DomainName != bpuaIdentifier.DomainName)
        {
currentRequestMetadata.DomainName = bpuaIdentifier.DomainName;
        }

        if (currentRequestMetadata.StateName != bpuaIdentifier.StateName)
        {
currentRequestMetadata.StateName = bpuaIdentifier.StateName;
        }

        if (currentRequestMetadata.TransitionName != bpuaIdentifier.TransitionName)
        {
currentRequestMetadata.TransitionName = bpuaIdentifier.TransitionName;
        }

        if (currentRequestMetadata.UseCaseName != bpuaIdentifier.UseCaseName)
        {
currentRequestMetadata.UseCaseName = bpuaIdentifier.UseCaseName;
        }
    }
}
        }
        #endregion
    }
}
