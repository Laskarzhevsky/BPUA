using PocoDataSet.IData;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines state handler functionality
    /// </summary>
    public interface IStateHandler : IRequestHandler
    {
        #region Properties
        /// <summary>
        /// Gets or sets data set
        /// </summary>
        new IDataSet? DataSet
        {
            get; set;
        }
        #endregion
    }
}
