using BPUA.Application.Contracts;
using BPUA.Core;

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
        /// Gets BPUA identifier
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <returns>BPUA identifier</returns>
        public static IBPUAIdentifier GetBPUAIdentifier(this IDataSet dataSet)
        {
            if (dataSet == null)
            {
                return null;
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

            IBPUAIdentifier bpuaIdentifier = dataRow.ToPoco<BPUAIdentifier>();
            return bpuaIdentifier;
        }
        #endregion
    }
}
