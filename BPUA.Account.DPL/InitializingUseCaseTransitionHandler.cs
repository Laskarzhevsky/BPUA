using BPUA.Application.Contracts;
using BPUA.Application.DataProcessingLogic;
using BPUA.Core;

using PocoDataSet.IData;

namespace BPUA.Account.DPL
{
    [RegisterAsBPUAService]
    public class InitializingUseCaseTransitionHandler : DataProcessingLogicTransitionHandler, IDataProcessingLogicTransitionHandler
    {
        #region Identification
        public static string DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
        public static string UseCaseName = BPUA.Account.Contracts.UseCaseName.ACCOUNT;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DPL;
        public static string StateName = default!;
        public static string TransitionName = BPUA.Application.Contracts.TransitionsNames.INITIALIZING_USE_CASE;

        /// <summary>
        /// Gets service key
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
        #region Public Methods
        /// <summary>
        /// Processes response
        /// </summary>
        protected override void ProcessResponse()
        {
            IDataTable? dataTable = ResponseDataSet!["Employee"];
            dataTable!.AddNewRow();
        }
        #endregion
*/
    }
}
