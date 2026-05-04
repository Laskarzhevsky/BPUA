using BPUA.Application.Contracts;
using BPUA.Core;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Provides request to the next layer event handler functionality
    /// </summary>
    public partial class TransitionContextRouter
    {
        #region Private Methods
        /// <summary>
        /// Checks if the transition handler specified in the BPUA identifier is registered in the service registry and retrieves it
        /// </summary>
        /// <returns>True if the transition handler is registered; otherwise, false.</returns>
        bool TransitionHandlerRegistered()
        {
            string handlerTypeKey = KeyCompiler.CompileTransitionHandlerKey(BpuaIdentifier.DomainName, BpuaIdentifier.UseCaseName, BpuaIdentifier.ApplicationLayerName, BpuaIdentifier.StateName, BpuaIdentifier.TransitionName);
            ITransitionHandler? transitionHandler = BpuaApplication!.GetRequestHandler(handlerTypeKey) as ITransitionHandler;
            if (transitionHandler == null)
            {
                return false;
            }

            TransitionHandler = transitionHandler;
            return true;
        }
        #endregion
    }
}
