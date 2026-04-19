using BPUA.Application.Contracts;
using BPUA.Application.StateMachineComponents;
using BPUA.Core;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.IData;

namespace HR.Application.SL
{
    /// <summary>
    /// Sends request to application next layer to execute InitializingApplication transition from the WaitingForApplicationLoad state of the Application use case of the HR application located in the state logic application layer
    /// </summary>
    [RegisterAsTransition]
    public class SendRequestToApplicationNextLayer_HR_Application_SL_WaitingForApplicationLoad_InitializingApplication : Transition
    {
        #region Identification
        public static string RequestName = BPUA.Application.Contracts.RequestNames.SEND_REQUEST_TO_APPLICATION_NEXT_LAYER;
        public static string DomainName = HR.Application.Contracts.Contract.HR;
        public static string UseCaseName = HR.Application.Contracts.UseCaseNames.APPLICATION;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.SL;
        public static string StateName = HR.Application.Contracts.StateNames.WAITING_FOR_APPLICATION_LOAD;
        public static string TransitionName = HR.Application.Contracts.TransitionsNames.INITIALIZING_APPLICATION;

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
        public SendRequestToApplicationNextLayer_HR_Application_SL_WaitingForApplicationLoad_InitializingApplication() : base(RequestName, DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName)
        {
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Processes the request transition context
        /// </summary>
        /// <param name="requestTransitionContext">Request transition context</param>
        /// <param name="bpuaIdentifier">BPUA identifier</param>
        public override void ProcessRequestTransitionContext(IDataSet requestTransitionContext, IBPUAIdentifier bpuaIdentifier)
        {
            bpuaIdentifier.TransitionName = BpuaIdentifier.TransitionName;

            IBPUAIdentifier nextApplicationLayerBpuaIdentifier = bpuaIdentifier.Clone()!;
            nextApplicationLayerBpuaIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.BL;
            requestTransitionContext.AddRequestMetadata(nextApplicationLayerBpuaIdentifier);
        }

        /// <summary>
        /// Processes the response transition context
        /// </summary>
        /// <param name="responseTransitionContext">Response transition context</param>
        public override void ProcessResponseTransitionContext(IDataSet responseTransitionContext)
        {
            responseTransitionContext.RemoveLastRequestMetadata();
        }
        #endregion
    }
}
