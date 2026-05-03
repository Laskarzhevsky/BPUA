using BPUA.Application.Contracts;
using BPUA.Application.NonFunctionalContracts;
using BPUA.Application.StateMachineComponents;
using BPUA.Application.Validation;
using BPUA.Core;

namespace BPUA.InfrastructureServer.DPL
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
                IBPUAIdentifier bpuaIdentifier = BPUA.InfrastructureServer.Contracts.Endpoints.RegisteringHost();
                return KeyCompiler.CompileTransitionKey(bpuaIdentifier.RequestName, bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName, bpuaIdentifier.StateName, bpuaIdentifier.TransitionName);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public RegisteringHostEndpointTransition() : base(default!, BPUA.InfrastructureServer.Contracts.Endpoints.RegisteringHost())
        {
            IsEndpoint = true;
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Adds request data context validation rules
        /// </summary>
        protected override void AddRequestDataContextValidationRules()
        {
            RequestDataContextValidationRules.Add(new AtLeastOneRowMustExistInDataTable(typeof(IHostedApplicationLayer).Name, ComponentIdentifier));
            RequestDataContextValidationRules.Add( new AnyNumberOfRowsMayExistInDataTable(BPUA.Application.Contracts.TableNames.INFRASTRUCTURE_SERVER + BPUA.Application.Contracts.TableNames.TRANSITION_HANDLER, ComponentIdentifier));
        }

        /// <summary>
        /// Adds response data context validation rules
        /// </summary>
        protected override void AddResponseDataContextValidationRules()
        {
            RequestDataContextValidationRules.Add(new OneOnlyRowMustExistInDataTable(BPUA.Application.Contracts.TableNames.INFRASTRUCTURE_SERVER + BPUA.Application.Contracts.TableNames.HOST_SUFFIX, ComponentIdentifier));
        }

        /// <summary>
        /// Prepares the BPUA identifier for the next transition handler
        /// </summary>
        /// <param name= "nextTransitionHandlerBpuaIdentifier" >Next transition handler BPUA identifier</param>
        protected override void PrepareNextTransitionHandlerBpuaIdentifier(IBPUAIdentifier nextTransitionHandlerBpuaIdentifier)
        {
            nextTransitionHandlerBpuaIdentifier.StateName = BPUA.Application.Contracts.StateNames.INITIAL;
        }
        #endregion
    }
}
