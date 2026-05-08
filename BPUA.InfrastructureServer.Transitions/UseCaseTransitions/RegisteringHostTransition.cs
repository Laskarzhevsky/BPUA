using BPUA.Application.Contracts;
using BPUA.Application.NonFunctionalContracts;
using BPUA.Application.StateMachineComponents;
using BPUA.Application.Validation;
using BPUA.Core;

namespace BPUA.InfrastructureServer.Transitions
{
    /// <summary>
    /// Provides RegisteringHost endpoint functionality
    /// </summary>
    [RegisterAsTransition]
    public class RegisteringHostTransition : SendRequestToApplicationNextLayerTransition
    {
        #region Identification
        public static string RequestName = BPUA.Application.Contracts.RequestNames.SEND_REQUEST_TO_APPLICATION_NEXT_LAYER;
        public static string DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
        public static string UseCaseName = BPUA.InfrastructureServer.Contracts.UseCaseName.INFRASTRUCTURE_SERVER;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DPL;
        public static string StateName = BPUA.Application.Contracts.StateNames.INITIAL;
        public static string TransitionName = BPUA.InfrastructureServer.Contracts.TransitionsNames.REGISTERING_HOST;

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
        public RegisteringHostTransition() : base(RequestName, DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName)
        {
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Adds request data context validation rules
        /// </summary>
        protected override void AddRequestDataContextValidationRules()
        {
            RequestDataContextValidationRules.Add(new AtLeastOneRowMustExistInDataTable(typeof(IHostedApplicationLayer).Name, ComponentIdentifier));
        }

        /// <summary>
        /// Adds response data context validation rules
        /// </summary>
        protected override void AddResponseDataContextValidationRules()
        {
        }

        /// <summary>
        /// Prepares the BPU identifier for the next transition handler
        /// </summary>
        /// <param name= "nextTransitionHandlerBpuIdentifier" >Next transition handler BPU identifier</param>
        protected override void PrepareNextTransitionHandlerBpuIdentifier(IBpuIdentifier nextTransitionHandlerBpuIdentifier)
        {
            base.PrepareNextTransitionHandlerBpuIdentifier(nextTransitionHandlerBpuIdentifier);
        }
        #endregion
    }
}
