using BPUA.Core;

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
        /// Routes the transition context.
        /// </summary>
        async Task RouteTransitionContext()
        {
            InitializeTransitionContextRouter();

            if (!HostedApplicationLayerRegistered())
            {
                string hostedApplicationLayerKey = KeyCompiler.CompileHostedApplicationLayerKey(BpuIdentifier.DomainName, BpuIdentifier.UseCaseName, BpuIdentifier.ApplicationLayerName);
                throw new InvalidOperationException("Hosted application layer is not registered. Key='" + hostedApplicationLayerKey + "'.");
            }

            await UseCaseActivated();

            if (!await PrepareTransitionContextForRedirection())
            {
                if (LastUseCaseActivationResult != null && !UseCaseActivationSucceeded(LastUseCaseActivationResult))
                {
                    string activationFailureMessage = BuildUseCaseActivationFailureMessage(LastUseCaseActivationResult);
                    throw new InvalidOperationException(activationFailureMessage);
                }

                throw new InvalidOperationException("Transition context was not prepared for redirection.");
            }

            if (!RequestHandlerRegistered())
            {
                string requestHandlerTypeKey = KeyCompiler.CompileRequestHandlerKey(BpuIdentifier.DomainName, BpuIdentifier.UseCaseName, BpuIdentifier.ApplicationLayerName, BpuIdentifier.StateName, BpuIdentifier.TransitionName);
                throw new InvalidOperationException("Request handler is not registered. Key='" + requestHandlerTypeKey + "'.");
            }

            await RedirectTransitionContextToRequestHandler();
            RequestRoute.ProcessResponseTransitionContext(ResponseTransitionContext!);
        }
        #endregion
    }
}
