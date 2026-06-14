using BPUA.Application.NonFunctionalContracts;

using PocoDataSet.IData;
using PocoDataSet.Extensions;

namespace BPUA.InfrastructureServer.DPL
{
    /// <summary>
    /// RegisteringHost service handler
    /// </summary>
    public partial class RegisteringHostRequestHandler
    {
        #region Private Methods
        /// <summary>
        /// Merges application layers from request with found
        /// </summary>
        void MergeApplicationLayersFromRequestWithFound()
        {
            IDataTable hostedApplicationLayerTable = RequestTransitionContext!.Tables[typeof(IHostedApplicationLayer).Name];
            if (ResponseTransitionContext!.TryGetTable(hostedApplicationLayerTable.TableName.Substring(1), out IDataTable? foundHostedApplicationLayerTable))
            {
                // Delete all registered application layers, as they will be replaced by the ones from the request
                for (int i = foundHostedApplicationLayerTable!.Rows.Count - 1; i >= 0; i--)
                {
                    foundHostedApplicationLayerTable.DeleteRowAt(i);
                }

                // Add application layers from request to found
                for (int i = 0; i < hostedApplicationLayerTable.Rows.Count; i++)
                {
                    IDataRow hostedApplicationLayerRow = hostedApplicationLayerTable.Rows[i];
                    IDataRow newDataRow = foundHostedApplicationLayerTable.AddNewRow();
                    newDataRow.CopyFrom(hostedApplicationLayerRow, hostedApplicationLayerTable.Columns);
                }
            }
        }
        #endregion
    }
}
