/*
using BPUA.Application.Contracts;
using BPUA.Application.StateMachineComponents;
using BPUA.Core;

using PocoDataSet.IData;

namespace BPUA.InfrastructureServer.SL
{
    /// <summary>
    /// Sends request to application next layer to execute InitializingApplication transition from the WaitingForApplicationLoad state of the Application use case of the HR application located in the state logic application layer
    /// </summary>
    [RegisterAsTransition]
    public class SendRequestToApplicationNextLayer_BPUA_InfrastructureServer_SL_WaitingForApplicationLoad_InitializingApplication: SendRequestToApplicationNextLayerTransition
    {
        #region Identification
        public static string RequestName = BPUA.Application.Contracts.RequestNames.SEND_REQUEST_TO_APPLICATION_NEXT_LAYER;
        public static string DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
        public static string UseCaseName = BPUA.InfrastructureServer.Contracts.UseCaseName.INFRASTRUCTURE_SERVER;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.SL;
        public static string StateName = BPUA.Application.Contracts.StateNames.WAITING_FOR_APPLICATION_LOAD;
        public static string TransitionName = BPUA.Application.Contracts.TransitionsNames.INITIALIZING_APPLICATION;

        /// <summary>
        /// Gets transition key
        /// </summary>
        public static string TransitionKey
        {
            get
            {
                return KeyCompiler.CompileTransitionKey(RequestName, DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName);
            }
        }
        #endregion

        #region Static Constructors
        /// <summary>
        /// Static constructor
        /// </summary>
        public static bool IsDefaultForState
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public SendRequestToApplicationNextLayer_BPUA_InfrastructureServer_SL_WaitingForApplicationLoad_InitializingApplication() : base(RequestName, DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName)
        {
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Prepares the BPUA identifier for the next transition handler
        /// </summary>
        /// <param name= "nextTransitionHandlerBpuaIdentifier" >Next transition handler BPUA identifier</param>
        protected override void PrepareNextTransitionHandlerBpuaIdentifier(IBPUAIdentifier nextTransitionHandlerBpuaIdentifier)
        {
            // Add transition name from the BPUA identifier to the request data context
            nextTransitionHandlerBpuaIdentifier.TransitionName = BpuaIdentifier.TransitionName;
        }
        #endregion
    }
}
*/