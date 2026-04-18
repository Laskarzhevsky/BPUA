using BPUA.Application.Contracts;
using BPUA.Application.StateMachineComponents;

using PocoDataSet.IData;

using System.Threading.Tasks;

namespace BPUA.Application.DataAccessLogic
{
    /// <summary>
    /// Provides transition handler functionality
    /// </summary>
    public abstract class DataAccessLogicTransitionHandler : TransitionHandler, IDataAccessLogicTransitionHandler
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public DataAccessLogicTransitionHandler() : base()
        {
        }

        /// <summary>
        /// Creates an instance, taking the transition handler identity as arguments
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">Transition name</param>
        public DataAccessLogicTransitionHandler(string domainName, string useCaseName, string applicationLayerName, string stateName, string transitionName) : base(domainName, useCaseName, applicationLayerName, stateName, transitionName)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Handles request
        /// </summary>
        /// <param name="requestTransitionContext">Request transition context</param>
        /// <returns>Response transition context</returns>
        public override async Task<IDataSet?> HandleRequestAsync(IDataSet? requestTransitionContext)
        {
            RequestTransitionContext = requestTransitionContext;
            ResponseTransitionContext = requestTransitionContext;
            if (requestTransitionContext == null)
            {
                return ResponseTransitionContext;
            }

            ProcessRequest();

            return ResponseTransitionContext;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Processes response
        /// Hide ProcessResponse method from inherited classes
        /// </summary>
        protected new void ProcessResponse()
        {
        }
        #endregion
    }
}
