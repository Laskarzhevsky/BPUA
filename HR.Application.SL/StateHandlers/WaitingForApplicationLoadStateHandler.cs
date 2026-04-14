using BPUA.Application.CommonComponents;
using BPUA.Application.Contracts;
using BPUA.Core;

namespace HR.Application.SL
{
    /// <summary>
    /// Provides functionality of the state handler for "Initial" state of the account use case in the state logic application layer
    /// </summary>
    [RegisterAsBPUAService]
    public class WaitingForApplicationLoadStateHandler : StateHandler
    {
        #region Identification
        public static string DomainName = HR.Application.Contracts.Contract.HR;
        public static string UseCaseName = HR.Application.Contracts.UseCaseNames.APPLICATION;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.SL;
        public static string StateName = HR.Application.Contracts.StateNames.WAITING_FOR_APPLICATION_LOAD;

        /// <summary>
        /// Gets service keys
        /// </summary>
        public static string ServiceKey
        {
            get
            {
                return KeyCompiler.CompileStateHandlerKey(DomainName, UseCaseName, ApplicationLayerName, StateName);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public WaitingForApplicationLoadStateHandler() : base(DomainName, UseCaseName, ApplicationLayerName, StateName)
        {
        }
        #endregion
    }
}
