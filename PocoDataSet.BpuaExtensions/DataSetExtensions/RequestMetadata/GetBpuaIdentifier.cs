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
        /// Gets BPUA identifier
        /// </summary>
        /// <param name="dataSet">Data set</param>
        /// <returns>BPUA identifier</returns>
        public static IBPUAIdentifier? GetBpuaIdentifier(this IDataSet? dataSet)
        {
            if (dataSet == null)
            {
                return null;
            }

            IRequestMetadata? requestMetadata = dataSet.GetRequestMetadata();
            return requestMetadata as IBPUAIdentifier;
        }
        #endregion
    }
}
