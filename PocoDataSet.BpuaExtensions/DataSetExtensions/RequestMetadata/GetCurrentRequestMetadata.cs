using BPUA.Application.Contracts;

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
        /// Gets current request metadata
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <returns>Current request metadata</returns>
        public static IRequestMetadata? GetCurrentRequestMetadata(this IDataSet? dataSet)
        {
            if (dataSet == null)
            {
                return null;
            }

            IDataTable requestMetadataDataTable = dataSet.GetRequestMetadataDataTable();
            if (requestMetadataDataTable.Rows.Count == 0)
            {
                return null;
            }

            IRequestMetadata requestMetadata = requestMetadataDataTable.Rows[requestMetadataDataTable.Rows.Count - 1].AsInterface<IRequestMetadata>();
            return requestMetadata;
        }
        #endregion
    }
}
