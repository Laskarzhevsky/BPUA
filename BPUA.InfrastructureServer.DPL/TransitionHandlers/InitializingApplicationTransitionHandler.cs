using BPUA.Application.DataProcessingLogic;
using BPUA.Application.Contracts;
using BPUA.Core;

namespace BPUA.InfrastructureServer.DPL
{
    /// <summary>
    /// Provides functionality of the transition handler for "Initializing use case" transition of the account use case in the data processing logic application layer
    /// </summary>
    [RegisterAsBpuaService]
    public class InitializingApplicationTransitionHandler : DataProcessingLogicTransitionHandler, IDataProcessingLogicTransitionHandler
    {
        #region Identification
        public static string DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
        public static string UseCaseName = BPUA.InfrastructureServer.Contracts.UseCaseName.INFRASTRUCTURE_SERVER;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DPL;
        public static string StateName = BPUA.Application.Contracts.StateNames.WAITING_FOR_APPLICATION_LOAD;
        public static string TransitionName = BPUA.Application.Contracts.TransitionsNames.INITIALIZING_APPLICATION;

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
        public InitializingApplicationTransitionHandler() : base(DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName)
        {
            DoNotSendRequestToApplicationNextLayer = true;
        }
        #endregion
    }
}
