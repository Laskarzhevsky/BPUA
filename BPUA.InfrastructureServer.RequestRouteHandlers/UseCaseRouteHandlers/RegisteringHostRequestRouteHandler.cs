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
    public class RegisteringHostRequestRouteHandler : RequestRouteHandler
    {
        #region Identification
        /// <summary>
        /// Gets request route key
        /// </summary>
        public static string RequestRouteKey
        {
            get
            {
                return KeyCompiler.CompileRequestRouteKey(BPUA.InfrastructureServer.Contracts.RequestNames.REGISTER_HOST, BPUA.InfrastructureServer.Contracts.DPL.RequestHandlers.BpuIdentifiers.RegisteringHost);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public RegisteringHostRequestRouteHandler() : base(RequestRouteKey)
        {
            TargetRequestHandlerBpuIdentifier = BPUA.InfrastructureServer.Contracts.DAL.RequestHandlers.BpuIdentifiers.RegisteringHost;
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
        #endregion
    }
}
