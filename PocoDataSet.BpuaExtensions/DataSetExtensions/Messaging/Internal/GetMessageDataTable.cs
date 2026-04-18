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
        /// Gets message data table
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <returns>Message data table</returns>
        internal static IDataTable GetMessageDataTable (this IDataSet? dataSet)
        {
            if (dataSet == null)
            {
                return default!;
            }

            IDataTable? messageDataTable = null;
            dataSet.TryGetTable(BPUA.Application.Contracts.TableNames.MESSAGE, out messageDataTable);
            if (messageDataTable == null)
            {
                messageDataTable = dataSet.AddNewTableFromPocoInterface(BPUA.Application.Contracts.TableNames.MESSAGE, typeof(IMessage));
            }

            return messageDataTable;
        }
        #endregion
    }
}
