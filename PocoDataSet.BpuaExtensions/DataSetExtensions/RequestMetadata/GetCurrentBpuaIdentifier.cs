using BPUA.Application.Contracts;
using BPUA.Core;

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
        /// Gets current BPU identifier
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <returns>Current BPU identifier</returns>
        public static IBpuIdentifier? GetCurrentBpuIdentifier(this IDataSet? dataSet)
        {
            if (dataSet == null)
            {
                return null;
            }

            IRequestMetadata? requestMetadata = dataSet.GetCurrentRequestMetadata();
            return requestMetadata as IBpuIdentifier;
        }
        #endregion
    }
}
