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
        /// Gets transition metadata data table
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <returns>Transition metadata data table</returns>
        internal static IDataTable GetTransitionMetadataDataTable(this IDataSet? dataSet)
        {
if (dataSet == null)
{
    return default!;
}

IDataTable? transitionMetadataDataTable = null;
dataSet.TryGetTable(BPUA.Application.Contracts.TableNames.TRANSITION_METADATA, out transitionMetadataDataTable);
if (transitionMetadataDataTable == null)
{
    transitionMetadataDataTable = dataSet.AddNewTableFromPocoInterface(BPUA.Application.Contracts.TableNames.TRANSITION_METADATA, typeof(ITransitionMetadata));
}

return transitionMetadataDataTable;
        }
        #endregion
    }
}
