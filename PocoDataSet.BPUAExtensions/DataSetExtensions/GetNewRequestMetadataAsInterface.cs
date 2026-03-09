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
        /// Gets new request metadata
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <returns>New request metadata</returns>
        public static IRequestMetadata GetNewRequestMetadataAsInterface(this IDataSet dataSet)
        {
            IDataTable requestMetadataDataTable = dataSet.GetRequestMetadataTable();
            IDataRow dataRow = requestMetadataDataTable.AddNewRow();
            IRequestMetadata requestMetadata = DataRowExtensions.AsInterface<IRequestMetadata>(dataRow);

            return requestMetadata;
        }
        #endregion
    }
}
