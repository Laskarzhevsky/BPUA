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
        /// Gets caller BPUA identifier
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <returns>Caller BPUA identifier</returns>
        public static IBPUAIdentifier? GetCallerBpuaIdentifier(this IDataSet? dataSet)
        {
            if (dataSet == null)
            {
                return null;
            }

            IRequestMetadata? requestMetadata = dataSet.GetCallerRequestMetadata();
            return requestMetadata as IBPUAIdentifier;
        }
        #endregion
    }
}
