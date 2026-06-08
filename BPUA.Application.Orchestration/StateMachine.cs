using BPUA.Application.Contracts;
using BPUA.Core;

using System;
using System.Text;
using System.Threading.Tasks;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Provides state machine functionality
    /// </summary>
    public class StateMachine
    {
        #region Methods
        /// <summary>
        /// Starts state machine
        /// </summary>
        /// <param name="bpuaApplication">BPUA application instance</param>
        /// <param name="bpuIdentifier">BPU identifier</param>
        public async Task ExecuteTransition(BpuaApplication bpuaApplication, IBpuIdentifier bpuIdentifier)
        {
            UseCaseActivationResult useCaseActivationResult = await bpuaApplication.ActivateUseCaseAsync(bpuIdentifier);
            if (UseCaseActivationSucceeded(useCaseActivationResult))
            {
                string bpuaServicekey = KeyCompiler.CompileStateHandlerKey(bpuIdentifier.DomainName, bpuIdentifier.UseCaseName, bpuIdentifier.ApplicationLayerName, bpuIdentifier.StateName);
                IBpuaService? bpuaService = bpuaApplication.GetRequestHandler(bpuaServicekey);
                if (bpuaService == null)
                {
                    throw new InvalidOperationException($"State handler with key '{bpuaServicekey}' is not found for hosted application layer with key '{KeyCompiler.CompileHostedApplicationLayerKey(bpuIdentifier.DomainName, bpuIdentifier.UseCaseName, bpuIdentifier.ApplicationLayerName)}'.");
                }
                else
                {
                    if (bpuaService is IStateHandler)
                    {
                        IStateHandler stateHandler = (IStateHandler)bpuaService;
                        await stateHandler.Initialize();
                        if (bpuIdentifier.StateName != stateHandler.BpuIdentifier.StateName)
                        {
                            await ExecuteTransition(bpuaApplication, stateHandler.BpuIdentifier);
                        }
                    }
                }
            }
            else
            {
                string message = BuildUseCaseActivationFailureMessage(bpuIdentifier, useCaseActivationResult);
                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        /// Checks whether the use case activation result represents successful activation.
        /// </summary>
        /// <param name="useCaseActivationResult">Use case activation result.</param>
        /// <returns>True if activation succeeded and has no errors; otherwise, false.</returns>
        bool UseCaseActivationSucceeded(UseCaseActivationResult useCaseActivationResult)
        {
            bool succeeded = useCaseActivationResult.Succeeded;

            if (useCaseActivationResult.Errors != null && useCaseActivationResult.Errors.Count > 0)
            {
                succeeded = false;
            }

            return succeeded;
        }

        /// <summary>
        /// Builds detailed use case activation failure message.
        /// </summary>
        /// <param name="bpuIdentifier">BPU identifier.</param>
        /// <param name="useCaseActivationResult">Use case activation result.</param>
        /// <returns>Detailed failure message.</returns>
        string BuildUseCaseActivationFailureMessage(IBpuIdentifier bpuIdentifier, UseCaseActivationResult useCaseActivationResult)
        {
            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.Append("Use case activation failed.");
            messageBuilder.Append(Environment.NewLine);
            messageBuilder.Append("DomainName='");
            messageBuilder.Append(bpuIdentifier.DomainName);
            messageBuilder.Append("'. UseCaseName='");
            messageBuilder.Append(bpuIdentifier.UseCaseName);
            messageBuilder.Append("'. ApplicationLayerName='");
            messageBuilder.Append(bpuIdentifier.ApplicationLayerName);
            messageBuilder.Append("'. StateName='");
            messageBuilder.Append(bpuIdentifier.StateName);
            messageBuilder.Append("'. TransitionName='");
            messageBuilder.Append(bpuIdentifier.TransitionName);
            messageBuilder.Append("'.");

            if (useCaseActivationResult.Errors != null && useCaseActivationResult.Errors.Count > 0)
            {
                messageBuilder.Append(Environment.NewLine);
                messageBuilder.Append("Activation errors:");

                foreach (object error in useCaseActivationResult.Errors)
                {
                    messageBuilder.Append(Environment.NewLine);
                    messageBuilder.Append("- ");
                    messageBuilder.Append(error);
                }
            }

            return messageBuilder.ToString();
        }
        #endregion
    }
}
