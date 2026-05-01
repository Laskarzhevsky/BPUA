using BPUA.Application.Contracts;
using BPUA.Application.StateMachineComponents;
using BPUA.Application.Validation;
using BPUA.Core;

using PocoDataSet.IData;

namespace BPUA.InfrastructureServer.DPL
{
    /// <summary>
    /// Prepares the BPUA identifier for the next transition handler in the application layer initialization flow, after receiving a request to register a host for the application layer.
    /// </summary>
    [RegisterAsTransition]
    public class SendRequestToNextHandler_BPUA_InfrastructureServer_DPL__RegisteringHost : SendRequestToApplicationNextLayerTransition
    {
        #region Identification
        public static string RequestName = BPUA.Application.Contracts.RequestNames.SEND_REQUEST_TO_NEXT_HANDLER;
        public static string DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
        public static string UseCaseName = BPUA.InfrastructureServer.Contracts.UseCaseName.INFRASTRUCTURE_SERVER;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DPL;
        public static string StateName = default!;
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
        public SendRequestToNextHandler_BPUA_InfrastructureServer_DPL__RegisteringHost() : base(RequestName, DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName)
        {
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Adds request data context validation rules
        /// </summary>
        protected override void AddRequestDataContextValidationRules()
        {
            RequestDataContextValidationRules.Add(new OneOnlyRowMustExistInDataTable(BPUA.Application.Contracts.TableNames.INFRASTRUCTURE_SERVER + BPUA.Application.Contracts.TableNames.HOSTED_APPLICATION_LAYER, ComponentIdentifier));
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
