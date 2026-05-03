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
        /// Gets caller request metadata
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <returns>Caller request metadata</returns>
        public static IRequestMetadata? GetCallerRequestMetadata(this IDataSet? dataSet)
        {
            if (dataSet == null)
            {
                return null;
            }

            IDataTable requestMetadataDataTable = dataSet.GetRequestMetadataDataTable();
            if (requestMetadataDataTable.Rows.Count < 2)
            {
                return null;
            }

            IRequestMetadata requestMetadata = requestMetadataDataTable.Rows[requestMetadataDataTable.Rows.Count - 2].AsInterface<IRequestMetadata>();
            return requestMetadata;
        }
        #endregion
    }
}
