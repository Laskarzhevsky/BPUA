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
        /// Redirects transition context to the request handler
        /// </summary>
        async Task RedirectTransitionContextToRequestHandler()
        {
            RequestHandler.BpuaApplication = BpuaApplication!;
            await using (RequestHandler as IAsyncDisposable)
            {
                ((BpuaApplication)BpuaApplication!).SignInToRequestHandlerRequestServiceEvent(RequestHandler);
                ResponseTransitionContext = await RequestHandler.HandleRequestAsync(RequestTransitionContext);
                if (ResponseTransitionContext == null)
                {
                    throw new System.Exception("Transition handler did not return a response transition context.");
                }

                ((BpuaApplication)BpuaApplication!).SignOutFromRequestHandlerRequestServiceEvent(RequestHandler);
            }
        }
        #endregion
    }
}
