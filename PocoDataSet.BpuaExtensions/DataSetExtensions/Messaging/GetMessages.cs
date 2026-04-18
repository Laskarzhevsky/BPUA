using BPUA.Application.Contracts;

using PocoDataSet.Extensions;
using PocoDataSet.IData;

using System.Collections.Generic;

namespace PocoDataSet.BpuaExtensions
{
    /// <summary>
    /// Contains data set extension methods
    /// </summary>
    public static partial class DataSetExtensions
    {
        #region Public Methods
        /// <summary>
        /// Gets messages
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <returns>List of messages</returns>
        public static IList<IMessage> GetMessages(this IDataSet? dataSet)
        {
            if (dataSet == null)
            {
                return new List<IMessage>();
            }

            IDataTable messageDataTable = dataSet.GetMessageDataTable();
            IList<IMessage> messages = messageDataTable.ToList<IMessage>();

            return messages;
        }
        #endregion
    }
}
