using BPUA.Application.BusinessLogic;
using BPUA.Application.Contracts;
using BPUA.Core;

using PocoDataSet.BpuaExtensions;

namespace BPUA.InfrastructureServer.BL
{
    /// <summary>
    /// Provides functionality of the transition handler for "Initializing use case" transition of the account use case in the business logic application layer
    /// </summary>
    [RegisterAsBPUAService]
    public class InitializingApplicationTransitionHandler : BusinessLogicTransitionHandler, IBusinessLogicTransitionHandler
    {
        #region Identification
        public static string DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
        public static string UseCaseName = BPUA.InfrastructureServer.Contracts.UseCaseName.INFRASTRUCTURE_SERVER;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.BL;
        public static string StateName = BPUA.Application.Contracts.StateNames.WAITING_FOR_APPLICATION_LOAD;
        public static string TransitionName = BPUA.Application.Contracts.TransitionsNames.INITIALIZING_APPLICATION;

        /// <summary>
        /// Gets service keys
        /// </summary>
        public static string ServiceKey
        {
            get
            {
                return KeyCompiler.CompileTransitionHandlerKey(DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public InitializingApplicationTransitionHandler() : base(DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName)
        {
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Processes response
        /// </summary>
        protected override void ProcessResponse()
        {
            if (ResponseTransitionContext.HasError())
            {
                // TODO: send data context to BPUA Infrastructure Server for logging
                return;
            }
            else
            {
                IRequestMetadata? requestMetadata = ResponseTransitionContext.GetRequestMetadata();
                if (requestMetadata == null)
                {
                    throw new System.ApplicationException("Request metadata is missing in data set.");
                }

                requestMetadata.StateName = BPUA.Application.Contracts.StateNames.INITIAL;
            }
        }
        #endregion
    }
}
