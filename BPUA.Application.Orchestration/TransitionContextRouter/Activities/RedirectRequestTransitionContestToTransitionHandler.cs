using System;
using System.Threading.Tasks;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Provides request to the next layer event handler functionality
    /// </summary>
    public partial class TransitionContextRouter
    {
        #region Private Methods
        /// <summary>
        /// Redirects the request transition context to the transition handler and gets the response transition context
        /// </summary>
        async Task RedirectRequestTransitionContextToTransitionHandler()
        {
            TransitionHandler.BPUAApplication = BpuaApplication!;
            await using (TransitionHandler as IAsyncDisposable)
            {
                ((BPUAApplication)BpuaApplication!).SignInToRequestHandlerRequestServiceEvent(TransitionHandler);
                ResponseTransitionContext = await TransitionHandler.HandleRequestAsync(RequestTransitionContext);
                if (ResponseTransitionContext == null)
                {
                    throw new System.Exception("Transition handler did not return a response transition context.");
                }

                ((BPUAApplication)BpuaApplication!).SignOutFromRequestHandlerRequestServiceEvent(TransitionHandler);
            }
        }
        #endregion
    }
}
