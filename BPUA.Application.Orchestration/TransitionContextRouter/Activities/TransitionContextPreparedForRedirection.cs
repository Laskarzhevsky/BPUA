using BPUA.Application.Contracts;

using PocoDataSet.BpuaExtensions;

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
        /// Prepares the transition context for redirection
        /// </summary>
        /// <returns>True if the transition context is prepared for redirection; otherwise, false.</returns>
        async Task<bool> PrepareTransitionContextForRedirection()
        {
            GetRequestRouteFromServiceRegistry();
            RequestRoute.ProcessRequestTransitionContext(RequestTransitionContext);
            if (RequestTransitionContext.HasError())
            {
                return false;
            }

            BpuIdentifier = RequestTransitionContext.GetCurrentBpuIdentifier()!;
            UseCaseActivationResult useCaseActivationResult = await ((BpuaApplication)BpuaApplication!).ActivateUseCaseAsync(BpuIdentifier);
            if (!useCaseActivationResult.Succeeded || useCaseActivationResult.Errors.Count > 0)
            {
                string message = "Use case activation failed." + Environment.NewLine + string.Join(Environment.NewLine, useCaseActivationResult.Errors);
                throw new InvalidOperationException(message);
            }

            return true;
        }
        #endregion
    }
}
