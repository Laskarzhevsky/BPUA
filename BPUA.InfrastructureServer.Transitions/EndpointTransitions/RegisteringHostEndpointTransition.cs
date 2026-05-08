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
    public class RegisteringHostEndpointTransition : Transition
    {
        #region Identification
        /// <summary>
        /// Gets transition key
        /// </summary>
        public static string TransitionKey
        {
            get
            {
                IBpuIdentifier bpuIdentifier = BPUA.InfrastructureServer.Contracts.Endpoints.RegisteringHost();
                return KeyCompiler.CompileTransitionKey(bpuIdentifier.RequestName, bpuIdentifier.DomainName, bpuIdentifier.UseCaseName, bpuIdentifier.ApplicationLayerName, bpuIdentifier.StateName, bpuIdentifier.TransitionName);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public RegisteringHostEndpointTransition() : base(default!, BPUA.InfrastructureServer.Contracts.Endpoints.RegisteringHost())
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
            nextTransitionHandlerBpuIdentifier.StateName = BPUA.Application.Contracts.StateNames.INITIAL;
        }
        #endregion
    }
}
