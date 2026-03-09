using BPUA.Application.Contracts;

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
        /// Gets request metadata table
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <returns>Request metadata table</returns>
        public static IDataTable GetRequestMetadataTable(this IDataSet dataSet)
        {
            IDataTable? requestMetadataDataTable = null;
            if (dataSet.Tables.ContainsKey(ServiceTablesNames.REQUEST_METADATA))
            {
                requestMetadataDataTable = dataSet.Tables[ServiceTablesNames.REQUEST_METADATA];
            }
            else
            {
                requestMetadataDataTable = dataSet.AddRequestMetadataTable();
            }

            return requestMetadataDataTable;
        }
        #endregion
    }
}
