using BPUA.Application.Contracts;

using PocoDataSet.Extensions;
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
        /// Gets request metadata
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <returns>Request metadata</returns>
        public static IRequestMetadata GetRequestMetadata(this IDataSet? dataSet)
        {
            if (dataSet == null)
            {
                return default!;
            }

            IDataTable requestMetadataDataTable = dataSet.Tables[ServiceTablesNames.REQUEST_METADATA];
            IDataRow dataRow = default!;
            if (requestMetadataDataTable.Rows.Count == 0)
            {
                dataRow = requestMetadataDataTable.AddNewRow();
            }
            else
            {
                dataRow = requestMetadataDataTable.Rows[requestMetadataDataTable.Rows.Count - 1];
            }

            IRequestMetadata requestMetadata = DataRowExtensions.AsInterface<IRequestMetadata>(dataRow);
            return requestMetadata;
        }
        #endregion
    }
}
