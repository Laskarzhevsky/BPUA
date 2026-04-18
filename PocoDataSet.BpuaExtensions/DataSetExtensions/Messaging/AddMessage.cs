using BPUA.Application.Contracts;

using PocoDataSet.Extensions;
using PocoDataSet.IData;

using System;

namespace PocoDataSet.BpuaExtensions
{
    /// <summary>
    /// Contains data set extension methods
    /// </summary>
    public static partial class DataSetExtensions
    {
        #region Public Methods
        /// <summary>
        /// Adds exception to the data set
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <param name="messageType">Message type</param>
        /// <param name="exception">Exception instance</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="applicationLayerUrl">Application layer URL</param>
        /// <returns>Added message</returns>
        public static IMessage AddException(this IDataSet? dataSet, MessageType messageType, Exception? exception, string? applicationLayerName, string? applicationLayerUrl)
        {
            if (dataSet == null)
            {
                return default!;
            }

            IDataTable messageDataTable = dataSet.GetMessageDataTable();
            IDataRow messageDataRow = messageDataTable.AddNewRow();

            IMessage? newMessage = messageDataRow.AsInterface<IMessage>();
            newMessage.ApplicationLayerName = applicationLayerName;
            newMessage.ApplicationLayerUrl = applicationLayerUrl;
            newMessage.Exception = exception;
            newMessage.MessageType = MessageType.Error;

            return newMessage;
        }

        /// <summary>
        /// Adds message to the data set
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <param name="message">BPUA identifier</param>
        /// <returns>Added message</returns>
        public static IMessage AddMessage(this IDataSet? dataSet, IMessage message)
        {
            if (dataSet == null)
            {
                return default!;
            }

            IDataTable messageDataTable = dataSet.GetMessageDataTable();
            IDataRow messageDataRow = messageDataTable.AddNewRow();

            IMessage? newMessage = messageDataRow.AsInterface<IMessage>();
            newMessage.ApplicationLayerName = message.ApplicationLayerName;
            newMessage.ApplicationLayerUrl = message.ApplicationLayerUrl;
            newMessage.Exception = message.Exception;
            newMessage.MessageText = message.MessageText;
            newMessage.MessageType = message.MessageType;

            return newMessage;
        }

        /// <summary>
        /// Adds message to the data set
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <param name="messageType">Message type</param>
        /// <param name="messageText">Message text</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="applicationLayerUrl">Application layer URL</param>
        /// <returns>Added message</returns>
        public static IMessage AddMessage(this IDataSet? dataSet, MessageType messageType, string? messageText, string? applicationLayerName, string? applicationLayerUrl = null)
        {
            if (dataSet == null)
            {
                return default!;
            }

            IDataTable messageDataTable = dataSet.GetMessageDataTable();
            IDataRow messageDataRow = messageDataTable.AddNewRow();

            IMessage? newMessage = messageDataRow.AsInterface<IMessage>();
            newMessage.ApplicationLayerName = applicationLayerName;
            newMessage.ApplicationLayerUrl = applicationLayerUrl;
            newMessage.MessageText = messageText;
            newMessage.MessageType = messageType;

            return newMessage;
        }
        #endregion
    }
}
