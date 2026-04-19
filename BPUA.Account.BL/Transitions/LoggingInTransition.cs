using BPUA.Application.Contracts;
using BPUA.Application.StateMachineComponents;
using BPUA.Core;

using PocoDataSet.IData;

namespace BPUA.Account.BL
{
    [RegisterAsBPUAService]
    public class LoggingInTransition : Transition
    {
        #region Identification
        public static string RequestName = BPUA.Application.Contracts.RequestNames.SEND_REQUEST_TO_APPLICATION_NEXT_LAYER;
        public static string DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
        public static string UseCaseName = BPUA.Account.Contracts.Contract.ACCOUNT;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.SL;
        public static string StateName = BPUA.Application.Contracts.StateNames.INITIAL;
        public static string TransitionName = BPUA.Account.Contracts.TransitionsNames.LOGGING_IN;

        /// <summary>
        /// Gets service keys
        /// </summary>
        public static string ServiceKey
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
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">Transition name</param>
        public LoggingInTransition(string domainName, string useCaseName, string applicationLayerName, string stateName, string transitionName) : base(RequestName, domainName, useCaseName, applicationLayerName, stateName, transitionName)
        {
            AddTargetStateName(BPUA.Application.Contracts.StateNames.INITIAL);
            AddTargetStateName(BPUA.Account.Contracts.StateNames.LOGGED_IN);
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Processes the request transition context
        /// </summary>
        /// <param name="requestTransitionContext">Request transition context</param>
        /// <param name="bpuaIdentifier">BPUA identifier</param>
        public override void ProcessRequestTransitionContext(IDataSet requestTransitionContext, IBPUAIdentifier bpuaIdentifier)
        {
        }

        /// <summary>
        /// Processes the response transition context
        /// </summary>
        /// <param name="responseTransitionContext">Response transition context</param>
        public override void ProcessResponseTransitionContext(IDataSet responseTransitionContext)
        {
        }
        #endregion
    }
}
