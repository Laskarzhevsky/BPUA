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
        /// Gets new transition metadata
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <returns>New transition metadata</returns>
        public static ITransitionMetadata GetNewTransitionMetadataAsInterface(this IDataSet dataSet)
        {
            IDataTable transitionMetadataDataTable = dataSet.GetTransitionMetadataTable();
            IDataRow dataRow = transitionMetadataDataTable.AddNewRow();
            ITransitionMetadata transitionMetadata = DataRowExtensions.AsInterface<ITransitionMetadata>(dataRow);

            return transitionMetadata;
        }
        #endregion
    }
}
