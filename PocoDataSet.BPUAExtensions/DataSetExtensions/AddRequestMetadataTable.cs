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
        /// Adds request metadata table
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <returns>Request metadata table</returns>
        public static IDataTable AddRequestMetadataTable(this IDataSet dataSet)
        {
            IDataTable? requestMetadataDataTable = null;
            if (dataSet.Tables.ContainsKey(ServiceTablesNames.REQUEST_METADATA))
            {
                requestMetadataDataTable = dataSet.Tables[ServiceTablesNames.REQUEST_METADATA];
            }
            else
            {
                requestMetadataDataTable = dataSet.AddNewTable(ServiceTablesNames.REQUEST_METADATA);
                requestMetadataDataTable.AddColumnsFromInterface(typeof(IRequestMetadata));
                for (int i = 0; i < requestMetadataDataTable.Columns.Count; i++)
                {
                    IColumnMetadata columnMetadata = requestMetadataDataTable.Columns[i];
                    if (columnMetadata.ColumnName == nameof(IRequestMetadata.DomainName) ||
                        columnMetadata.ColumnName == nameof(IRequestMetadata.UseCaseName) ||
                        columnMetadata.ColumnName == nameof(IRequestMetadata.ApplicationLayerName) ||
                        columnMetadata.ColumnName == nameof(IRequestMetadata.StateName) ||
                        columnMetadata.ColumnName == nameof(IRequestMetadata.TransitionName) ||
                        columnMetadata.ColumnName == nameof(IRequestMetadata.Breadcrumbs)
                        )
                    {
                        columnMetadata.IsPrimaryKey = true;
                        columnMetadata.IsNullable = false;
                    }
                }

            }

            return requestMetadataDataTable;
        }
        #endregion
    }
}
