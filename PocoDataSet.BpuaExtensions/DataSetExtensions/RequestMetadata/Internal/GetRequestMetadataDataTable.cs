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
        /// Gets request metadata data table
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <returns>Request metadata dta table</returns>
        internal static IDataTable GetRequestMetadataDataTable(this IDataSet? dataSet)
        {
if (dataSet == null)
{
    return default!;
}

IDataTable? requestMetadataDataTable = null;
dataSet.TryGetTable(BPUA.Application.Contracts.TableNames.REQUEST_METADATA, out requestMetadataDataTable);
if (requestMetadataDataTable == null)
{
    requestMetadataDataTable = dataSet.AddNewTableFromPocoInterface(BPUA.Application.Contracts.TableNames.REQUEST_METADATA, typeof(IRequestMetadata));
}

return requestMetadataDataTable;
        }
        #endregion
    }
}
