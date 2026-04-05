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
                dataSet.RemoveRow(BPUA.Application.Contracts.TableNames.REQUEST_METADATA, requestMetadataDataTable.Rows.Count - 1);
            }
        }
        #endregion
    }
}
