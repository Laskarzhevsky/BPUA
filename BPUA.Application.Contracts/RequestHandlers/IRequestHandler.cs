using BPUA.Core;

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines request handler functionality
    /// </summary>
    public interface IRequestHandler : IDisposable, IBPUAService
    {
        #region Events
        /// <summary>
        /// Request service
        /// </summary>
        public event Func<object?, ServiceRequestEventArgs, Task>? ServiceRequestEvent;
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
        /// <param name="eventName">Event name</param>
        Task RaiseServiceRequestEventAsync(EventArgs args, [CallerMemberName] string eventName = "");
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
        /// Gets component identifier
        /// </summary>
        string ComponentIdentifier
        {
            get;
        }

        /// <summary>
        /// Gets or sets transition context
        /// </summary>
        ITransitionContext? TransitionContext
        {
            get; set;
        }
        #endregion
    }
}
