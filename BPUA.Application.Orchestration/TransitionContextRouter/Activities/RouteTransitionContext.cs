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
        /// Routes the transition context
        /// </summary>
        /// <param name="requestTransitionContext">Request transition context</param>
        async Task RouteTransitionContext()
        {
            InitializeTransitionContextRouter();
            if (HostedApplicationLayerRegistered())
            {
                if (await UseCaseActivated())
                {
                    if (await RequestTransitionContextPreparedForRedirection())
                    {
                        if (TransitionHandlerRegistered())
                        {
                            await RedirectRequestTransitionContextToTransitionHandler();
                            Transition.ProcessResponseTransitionContext(ResponseTransitionContext!);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
