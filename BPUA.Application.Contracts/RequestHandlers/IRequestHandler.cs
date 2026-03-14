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
        /// Handles request
        /// </summary>
        /// <param name="transitionContext">Request transition context</param>
        /// <returns>Response transition context</returns>
        Task<ITransitionContext?> HandleRequestAsync(ITransitionContext? transitionContext);

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
