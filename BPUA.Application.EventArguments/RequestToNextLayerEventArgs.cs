using System;

using PocoDataSet.IData;

namespace BPUA.Application.EventArguments
{
    /// <summary>
    /// Provides request to the next layer event arguments functionality
    /// </summary>
    public class RequestToNextLayerEventArgs : BPUAApplicationEventArgs
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataSet">Data set</param>
        public RequestToNextLayerEventArgs(IDataSet? dataSet)
        {
            DataSet = dataSet;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets data set
        /// </summary>
        public IDataSet? DataSet
        {
            get; set;
        }
        #endregion
    }
}
