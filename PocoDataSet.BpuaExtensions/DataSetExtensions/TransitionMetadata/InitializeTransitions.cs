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
        /// Initializes transitions
        /// </summary>
        /// <param name="dataSet">Data set</param>
        public static void InitializeTransitions(this IDataSet? dataSet)
        {
            if (dataSet == null)
            {
                return;
            }

            IDataTable transitionMetadataDataTable = GetTransitionMetadataDataTable(dataSet);
            List<ITransitionMetadata> transitionMetadataList = transitionMetadataDataTable.ToList<ITransitionMetadata>();
            for (int i = 0; i < transitionMetadataList.Count; i++)
            {
                ITransitionMetadata transitionMetadata = transitionMetadataList[i];
                transitionMetadata.Available = true;
            }
        }
        #endregion
    }
}
