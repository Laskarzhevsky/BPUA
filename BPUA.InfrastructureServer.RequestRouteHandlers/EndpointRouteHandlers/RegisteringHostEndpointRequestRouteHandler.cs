using BPUA.Application.Contracts;
using BPUA.Application.NonFunctionalContracts;
using BPUA.Application.ProcessComponents;
using BPUA.Application.Validation;
using BPUA.Core;

namespace BPUA.InfrastructureServer.RequestRouteHandlers
{
    /// <summary>
    /// Provides RegisteringHost endpoint functionality
    /// </summary>
    [RegisterAsRequestRoute]
    public class RegisteringHostEndpointRequestRouteHandler : RequestRouteHandler
    {
        #region Identification
        /// <summary>
        /// Gets request route key
        /// </summary>
        public static string RequestRouteKey
        {
            get
            {
                IBpuIdentifier bpuIdentifier = BPUA.InfrastructureServer.Contracts.Endpoints.RegisteringHostBpuIdentifier();
                return KeyCompiler.CompileRequestRouteKey(bpuIdentifier.RequestName, bpuIdentifier.DomainName, bpuIdentifier.UseCaseName, bpuIdentifier.ApplicationLayerName, bpuIdentifier.StateName, bpuIdentifier.TransitionName);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public RegisteringHostEndpointRequestRouteHandler() : base(default!, BPUA.InfrastructureServer.Contracts.Endpoints.RegisteringHostBpuIdentifier())
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
        /// Prepares the BPU identifier for the next request handler
        /// </summary>
        /// <param name= "nextRequestHandlerBpuIdentifier" >Next request handler BPU identifier</param>
        protected override void PrepareNextRequestHandlerBpuIdentifier(IBpuIdentifier nextRequestHandlerBpuIdentifier)
        {
            nextRequestHandlerBpuIdentifier.DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
            nextRequestHandlerBpuIdentifier.UseCaseName = BPUA.InfrastructureServer.Contracts.UseCaseName.INFRASTRUCTURE_SERVER;
            nextRequestHandlerBpuIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DPL;
            nextRequestHandlerBpuIdentifier.StateName = default!;
            nextRequestHandlerBpuIdentifier.TransitionName = BPUA.InfrastructureServer.Contracts.TransitionsNames.REGISTERING_HOST;
        }
        #endregion
    }
}
