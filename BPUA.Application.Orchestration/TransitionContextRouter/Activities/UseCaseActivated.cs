using BPUA.Application.Contracts;
using BPUA.Core;

using System;
using System.Text;
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
        /// Checks if the use case specified in the BPU identifier is activated, and if not, activates it.
        /// </summary>
        /// <returns>True if the use case is activated; otherwise, false.</returns>
        async Task<bool> UseCaseActivated()
        {
            UseCaseActivationResult useCaseActivationResult = await ((BpuaApplication)BpuaApplication!).ActivateUseCaseAsync(BpuIdentifier);
            LastUseCaseActivationResult = useCaseActivationResult;

            if (!UseCaseActivationSucceeded(useCaseActivationResult))
            {
                string message = BuildUseCaseActivationFailureMessage(useCaseActivationResult);
                throw new InvalidOperationException(message);
            }

            return true;
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
        /// <param name="useCaseActivationResult">Use case activation result.</param>
        /// <returns>Detailed failure message.</returns>
        string BuildUseCaseActivationFailureMessage(UseCaseActivationResult useCaseActivationResult)
        {
            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.Append("Use case activation failed.");
            messageBuilder.Append(Environment.NewLine);
            messageBuilder.Append("DomainName='");
            messageBuilder.Append(BpuIdentifier.DomainName);
            messageBuilder.Append("'. UseCaseName='");
            messageBuilder.Append(BpuIdentifier.UseCaseName);
            messageBuilder.Append("'. ApplicationLayerName='");
            messageBuilder.Append(BpuIdentifier.ApplicationLayerName);
            messageBuilder.Append("'. StateName='");
            messageBuilder.Append(BpuIdentifier.StateName);
            messageBuilder.Append("'. TransitionName='");
            messageBuilder.Append(BpuIdentifier.TransitionName);
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
