using BPUA.Application.BusinessLogic;
using BPUA.Application.Contracts;
using BPUA.Core;

namespace BPUA.Account.BusinessLogic
{
    [RegisterAsBPUAService]
    public class SearchingTransitionHandler : BusinessLogicTransitionHandler, IBusinessLogicTransitionHandler
    {
        #region Identification
        public static string DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
        public static string UseCaseName = BPUA.Account.Contracts.Contract.ACCOUNT;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.BL;
        public static string StateName = BPUA.Application.Contracts.StateNames.INITIAL;
        public static string TransitionName = BPUA.Account.Contracts.TransitionsNames.SEARCHING;

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
        public SearchingTransitionHandler() : base(DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName)
        {
        }
        #endregion
/*
        #region Public Methods
        /// <summary>
        /// Handles request
        /// </summary>
        /// <param name="requestDataSet">Request data set</param>
        /// <returns>Response data set</returns>
        public override async Task<IDataSet?> HandleRequestAsync(IDataSet? requestDataSet)
        {
            RequestToNextLayerEventArgs requestDispatchingEventArgs = new RequestToNextLayerEventArgs(requestDataSet);
            await RaiseServiceRequestEventAsync(requestDispatchingEventArgs);

            IDataSet? responseDataSet = requestDispatchingEventArgs.DataSet;
            return responseDataSet;
        }
        #endregion
*/
    }
}
