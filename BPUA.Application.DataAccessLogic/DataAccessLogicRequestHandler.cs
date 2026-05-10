using BPUA.Application.Contracts;
using BPUA.Application.ProcessComponents;
using BPUA.Core;

using PocoDataSet.IData;

using System.Threading.Tasks;

namespace BPUA.Application.DataAccessLogic
{
    /// <summary>
    /// Provides request route handler functionality
    /// </summary>
    public abstract class DataAccessLogicRequestHandler : RequestHandler, IDataAccessLogicRequestHandler
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public DataAccessLogicRequestHandler() : base()
        {
        }

        /// <summary>
        /// Creates an instance, taking the transition handler identity as arguments
        /// </summary>
        /// <param name="bpuIdentifier">BPU identifier</param>
        public DataAccessLogicRequestHandler(IBpuIdentifier bpuIdentifier) : base(bpuIdentifier)
        {
        }

        /// <summary>
        /// Creates an instance, taking the request route handler identity as arguments
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">Transition name</param>
        public DataAccessLogicRequestHandler(string domainName, string useCaseName, string applicationLayerName, string stateName, string transitionName) : base(domainName, useCaseName, applicationLayerName, stateName, transitionName)
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
            await ProcessRequestAsync();
            FinalizeTransitionContextProcessing();
            return ResponseTransitionContext;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Processes request
        /// </summary>
        protected override void ProcessRequest()
        {
        }

        /// <summary>
        /// Processes request asynchronously
        /// </summary>
        protected override async Task ProcessRequestAsync()
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Processes response
        /// Hide ProcessResponse method from inherited classes
        /// </summary>
        protected override void ProcessResponse()
        {
        }
        #endregion
    }
}
