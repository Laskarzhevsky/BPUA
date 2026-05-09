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
        /// Checks if the request handler specified in the BPU identifier is registered in the service registry and retrieves it
        /// </summary>
        /// <returns>True if the transition handler is registered; otherwise, false.</returns>
        bool RequestHandlerRegistered()
        {
            string requestHandlerTypeKey = KeyCompiler.CompileRequestHandlerKey(BpuIdentifier.DomainName, BpuIdentifier.UseCaseName, BpuIdentifier.ApplicationLayerName, BpuIdentifier.StateName, BpuIdentifier.TransitionName);
            IRequestHandler? requestHandler = BpuaApplication!.GetRequestHandler(requestHandlerTypeKey) as IRequestHandler;
            if (requestHandler == null)
            {
                return false;
            }

            RequestHandler = requestHandler;
            return true;
        }
        #endregion
    }
}
