using BPUA.Application.Contracts;
using BPUA.Application.StateMachineComponents;
using BPUA.Core;

namespace BPUA.Account.SL
{
    /// <summary>
    /// Provides functionality of the state handler for "Initial" state of the account use case in the state logic application layer
    /// </summary>
    [RegisterAsBPUAService]
    public class InitialStateHandler : StateHandler
    {
        #region Identification
        public static string DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
        public static string UseCaseName = BPUA.Account.Contracts.UseCaseName.ACCOUNT;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.SL;
        public static string StateName = BPUA.Application.Contracts.StateNames.INITIAL;

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
        public InitialStateHandler() : base(DomainName, UseCaseName, ApplicationLayerName, StateName)
        {
        }
        #endregion
    }
}
