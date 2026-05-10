using BPUA.Application.Contracts;
using BPUA.Application.ProcessComponents;
using BPUA.Core;

using PocoDataSet.IData;

using System.Threading.Tasks;

namespace BPUA.Application.DataProcessingLogic
{
    /// <summary>
    /// Provides request route handler functionality
    /// </summary>
    public abstract class DataProcessingLogicRequestHandler : RequestHandler, IDataAccessLogicRequestHandler
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public DataProcessingLogicRequestHandler() : base()
        {
        }

        /// <summary>
        /// Creates an instance, taking the transition handler identity as arguments
        /// </summary>
        /// <param name="bpuIdentifier">BPU identifier</param>
        public DataProcessingLogicRequestHandler(IBpuIdentifier bpuIdentifier) : base(bpuIdentifier)
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
    }
}
