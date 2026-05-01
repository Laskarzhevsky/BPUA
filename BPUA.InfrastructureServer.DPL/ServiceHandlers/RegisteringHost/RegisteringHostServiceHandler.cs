using BPUA.Application.Contracts;
using BPUA.Core;

namespace BPUA.InfrastructureServer.DPL
{
    /// <summary>
    /// RegisteringHost service handler
    /// </summary>
    [RegisterAsBPUAService]
    public partial class RegisteringHostServiceHandler : BPUA.Application.DataProcessingLogic.DataProcessingLogicTransitionHandler
    {
        #region Identification
        public static string DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
        public static string UseCaseName = BPUA.InfrastructureServer.Contracts.UseCaseName.INFRASTRUCTURE_SERVER;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DPL;
        public static string StateName = BPUA.Application.Contracts.StateNames.INITIAL;
        public static string TransitionName = BPUA.InfrastructureServer.Contracts.TransitionsNames.REGISTERING_HOST;

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
        public RegisteringHostServiceHandler() : base(DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName)
        {
        }
        #endregion
    }
}
