using BPUA.Application.BusinessLogic;
using BPUA.Application.Contracts;
using BPUA.Core;

namespace BPUA.Account.BL
{
    /// <summary>
    /// Provides functionality of the transition handler for "Initializing use case" transition of the account use case in the business logic application layer
    /// </summary>
    [RegisterAsBPUAService]
    public class InitializingUseCaseTransitionHandler : BusinessLogicTransitionHandler, IBusinessLogicTransitionHandler
    {
        #region Identification
        public static string DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
        public static string UseCaseName = BPUA.Account.Contracts.UseCaseName.ACCOUNT;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.BL;
        public static string StateName = default!;
        public static string TransitionName = BPUA.Application.Contracts.TransitionsNames.INITIALIZING_USE_CASE;

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
        public InitializingUseCaseTransitionHandler() : base(DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName)
        {
        }
        #endregion
/*
        #region Protected Methods
        /// <summary>
        /// Processes request
        /// </summary>
        protected override void ProcessRequest()
        {
            IDataSet responseDataSet = await DataProcessingLogicTransitionHandler.HandleRequest(requestDataSet);

            InitializingUseCaseTransitionValidator initializingUseCaseTransitionValidator = new InitializingUseCaseTransitionValidator();
            initializingUseCaseTransitionValidator.ApplyValidationRules(responseDataSet);
            List<string> validationErrors = initializingUseCaseTransitionValidator.Validate(responseDataSet);

            return responseDataSet;
        }
        #endregion
*/
    }
}
