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
        /// Gets flag indicating whether data set contains error
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <returns>Flag indicating whether data set contains error</returns>
        public static bool HasError(this IDataSet? dataSet)
        {
            if (dataSet == null)
            {
                return false;
            }

            if (dataSet.Tables.ContainsKey(BPUA.Application.Contracts.TableNames.MESSAGE))
            {
                IDataTable messageDataTable = dataSet.Tables[BPUA.Application.Contracts.TableNames.MESSAGE];
                foreach (IDataRow messageDataRow in messageDataTable.Rows)
                {
                    IMessage? message = messageDataRow.AsInterface<IMessage>();
                    if (message.MessageType == MessageType.Error)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion
    }
}
