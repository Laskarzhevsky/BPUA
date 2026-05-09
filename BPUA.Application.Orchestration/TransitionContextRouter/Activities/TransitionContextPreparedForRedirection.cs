using PocoDataSet.BpuaExtensions;

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
        /// Checks if the transition context is prepared for redirection
        /// </summary>
        /// <returns>True if the transition context is prepared for redirection; otherwise, false.</returns>
        async Task<bool> TransitionContextPreparedForRedirection()
        {
            GetRequestRouteFromServiceRegistry();
            RequestRoute.ProcessRequestTransitionContext(RequestTransitionContext);
            if (RequestTransitionContext.HasError())
            {
                return false;
            }

            BpuIdentifier = RequestTransitionContext.GetCurrentBpuIdentifier()!;
            bool useCaseActivated = await UseCaseActivated();

            return useCaseActivated;
        }
        #endregion
    }
}
