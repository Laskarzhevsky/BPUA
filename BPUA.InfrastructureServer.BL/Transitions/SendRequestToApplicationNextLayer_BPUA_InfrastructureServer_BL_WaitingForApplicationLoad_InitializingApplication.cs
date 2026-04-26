using BPUA.Application.Contracts;
using BPUA.Application.StateMachineComponents;
using BPUA.Core;

using PocoDataSet.IData;

namespace BPUA.InfrastructureServer.BL
{
    /// <summary>
    /// Sends request to application next layer to execute InitializingApplication transition from the WaitingForApplicationLoad state of the Application use case of the HR application located in the business logic application layer
    /// </summary>
    [RegisterAsTransition]
    public class SendRequestToApplicationNextLayer_BPUA_InfrastructureServer_BL_WaitingForApplicationLoad_InitializingApplication : SendRequestToApplicationNextLayerTransition
    {
        #region Identification
        public static string RequestName = BPUA.Application.Contracts.RequestNames.SEND_REQUEST_TO_APPLICATION_NEXT_LAYER;
        public static string DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
        public static string UseCaseName = BPUA.InfrastructureServer.Contracts.UseCaseNames.INFRASTRUCTURE_SERVER;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.BL;
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

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public SendRequestToApplicationNextLayer_BPUA_InfrastructureServer_BL_WaitingForApplicationLoad_InitializingApplication() : base(RequestName, DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName)
        {
        }
        #endregion
/*
        #region Overridden Methods
        /// <summary>
        /// Processes the request transition context
        /// </summary>
        /// <param name="requestTransitionContext">Request transition context</param>
        /// <param name="bpuaIdentifier">BPUA identifier</param>
        public override void ProcessRequestTransitionContext(IDataSet requestTransitionContext, IBPUAIdentifier bpuaIdentifier)
        {
            bpuaIdentifier.TransitionName = BpuaIdentifier.TransitionName;
            base.ProcessRequestTransitionContext(requestTransitionContext, bpuaIdentifier);
        }
        #endregion
*/
    }
}
