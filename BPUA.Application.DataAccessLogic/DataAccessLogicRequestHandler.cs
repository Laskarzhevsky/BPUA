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
        /// Creates an instance, taking the request handler key as arguments
        /// </summary>
        /// <param name="requestHandlerKey">Request handler key</param>
        public DataAccessLogicRequestHandler(string requestHandlerKey) : base(requestHandlerKey)
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
        /// Gets the connection string
        /// </summary>
        protected virtual void GetConnectionString()
        {
            IBpuaHostServices bpuaHostServices = (IBpuaHostServices)BpuaApplication;
            ConnectionString = bpuaHostServices.GetConnectionString(BpuIdentifier);
        }

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

        #region Properties
        /// <summary>
        /// Gets or sets the connection string
        /// </summary>
        protected string ConnectionString
        {
            get; set;
        }
        #endregion
    }
}
