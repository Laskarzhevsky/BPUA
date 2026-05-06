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
        /// Gets caller BPU identifier
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <returns>Caller BPU identifier</returns>
        public static IBpuIdentifier? GetCallerBpuIdentifier(this IDataSet? dataSet)
        {
            if (dataSet == null)
            {
                return null;
            }

            IRequestMetadata? requestMetadata = dataSet.GetCallerRequestMetadata();
            return requestMetadata as IBpuIdentifier;
        }
        #endregion
    }
}
