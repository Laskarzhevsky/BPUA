using System;
using System.Threading.Tasks;

using PocoDataSet.IData;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines request handler functionality
    /// </summary>
    public interface IRequestHandler : IDisposable, IBPUAService
    {
        #region Events
        /// <summary>
        /// Request service event
        /// </summary>
        public event Func<object?, EventArgs, Task>? RequestServiceEvent;
        #endregion

        #region Methods
        /// <summary>
        /// Gets new data set
        /// </summary>
        /// <returns>New data set</returns>
        IDataSet GetNewDataSet();

        /// <summary>
        /// Handles request
        /// </summary>
        /// <param name="requestDataSet">Request data set</param>
        /// <returns>Response data set</returns>
        Task<IDataSet?> HandleRequestAsync(IDataSet? requestDataSet);

        /// <summary>
        /// Raises service request event
        /// </summary>
        /// <param name="args">Event arguments</param>
        Task RaiseServiceRequestEventAsync(EventArgs args);
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets BPUA application
        /// </summary>
        IBPUAApplication BPUAApplication
        {
            get; set;
        }

        /// <summary>
        /// Gets request handler key
        /// </summary>
        string RequestHandlerKey
        {
            get;
        }

        /// <summary>
        /// Gets or sets data set
        /// </summary>
        IDataSet? DataSet
        {
            get; set;
        }
        #endregion
    }
}
