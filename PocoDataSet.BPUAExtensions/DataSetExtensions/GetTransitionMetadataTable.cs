using BPUA.Core;
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
        /// Gets transition metadata table
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <returns>Transition metadata table</returns>
        public static IDataTable GetTransitionMetadataTable(this IDataSet dataSet)
        {
            IDataTable? transitionMetadataDataTable = null;
            if (dataSet.Tables.ContainsKey(ServiceTablesNames.TRANSITION_METADATA))
            {
                transitionMetadataDataTable = dataSet.Tables[ServiceTablesNames.TRANSITION_METADATA];
            }
            else
            {
                transitionMetadataDataTable = dataSet.AddNewTable(ServiceTablesNames.TRANSITION_METADATA);
                transitionMetadataDataTable.AddColumnsFromInterface(typeof(ITransitionMetadata));
                for (int i = 0; i < transitionMetadataDataTable.Columns.Count; i++)
                {
                    IColumnMetadata columnMetadata = transitionMetadataDataTable.Columns[i];
                    if (columnMetadata.ColumnName == nameof(ITransitionMetadata.DomainName) ||
                        columnMetadata.ColumnName == nameof(ITransitionMetadata.UseCaseName) ||
                        columnMetadata.ColumnName == nameof(ITransitionMetadata.ApplicationLayerName) ||
                        columnMetadata.ColumnName == nameof(ITransitionMetadata.StateName) ||
                        columnMetadata.ColumnName == nameof(ITransitionMetadata.TransitionName) ||
                        columnMetadata.ColumnName == nameof(ITransitionMetadata.Breadcrumbs)
                        )
                    {
                        columnMetadata.IsPrimaryKey = true;
                        columnMetadata.IsNullable = false;
                    }
                }
            }

            return transitionMetadataDataTable;
        }
        #endregion
    }
}
