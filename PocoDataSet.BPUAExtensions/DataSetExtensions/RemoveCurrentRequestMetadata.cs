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
        /// Removes current request metadata
        /// </summary>
        /// <param name="dataSet">Data set</param>
        public static void RemoveCurrentRequestMetadata(this IDataSet dataSet)
        {
            IRequestMetadata currentRequestMetadata = dataSet!.GetRequestMetadata();

            // Copy current request metadata
            BPUAIdentifier bpuaIdentifier = new BPUAIdentifier();
            bpuaIdentifier.ApplicationLayerName = currentRequestMetadata.ApplicationLayerName;
            bpuaIdentifier.DomainName = currentRequestMetadata.DomainName;
            bpuaIdentifier.StateName = currentRequestMetadata.StateName;
            bpuaIdentifier.TransitionName = currentRequestMetadata.TransitionName;
            bpuaIdentifier.UseCaseName = currentRequestMetadata.UseCaseName;

            IDataTable requestMetadataDataTable = dataSet.Tables[ServiceTablesNames.REQUEST_METADATA];
            requestMetadataDataTable.RemoveRowAt(requestMetadataDataTable.Rows.Count - 1);

            // Propagate current use case matadata to previous for BL layer
            if (bpuaIdentifier.ApplicationLayerName == ApplicationLayersNames.BL)
            {
                currentRequestMetadata = dataSet!.GetRequestMetadata();
                if (currentRequestMetadata.DomainName != bpuaIdentifier.DomainName)
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
        #endregion
    }
}
