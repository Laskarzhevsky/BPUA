using System.Threading.Tasks;

using BPUA.Application.Contracts;
using BPUA.Application.DataAccessLogic;
using BPUA.Core;

using PocoDataSet.IData;

namespace BPUA.Account.DataAccessLogic
{
    [RegisterAsBPUAService]
    public class InitializingUseCaseTransitionHandler : DataAccessLogicTransitionHandler, IDataAccessLogicTransitionHandler
    {
        #region Identification
        public static string DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
        public static string UseCaseName = BPUA.Account.Contracts.Contract.ACCOUNT;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DAL;
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
        /// Handles request
        /// </summary>
        /// <param name="requestDataSet">Request data set</param>
        /// <returns>Response data set</returns>
        public override async Task<IDataSet?> HandleRequestAsync(IDataSet? requestDataSet)
        {
            SqlServerReadEventArgs sqlServerReadEventArgs = new SqlServerReadEventArgs("Employee.GetSearchCriteriaSchema", true, requestDataSet);
            await RaiseServiceRequestEventAsync(sqlServerReadEventArgs);

            IDataSet? responseDataSet = sqlServerReadEventArgs.DataSet;
            return responseDataSet;
        }
        #endregion
*/
    }
}
