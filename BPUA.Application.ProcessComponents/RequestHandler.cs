using BPUA.Application.Contracts;
using BPUA.Core;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.IData;

using System;
using System.Threading.Tasks;

namespace BPUA.Application.ProcessComponents
{
    public abstract class RequestHandler : AsyncDisposableObject, IRequestHandler, IBpuaService
    {
        #region Events
        /// <summary>
        /// Reqests service
        /// IRequestHandler interface implementation
        /// </summary>
        public event Func<object?, ServiceRequestEventArgs, Task>? ServiceRequestEvent;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public RequestHandler()
        {
        }

        /// <summary>
        /// Creates an instance, taking the request handler key as arguments
        /// </summary>
        /// <param name="requestHandlerKey">Request handler key</param>
        public RequestHandler(string requestHandlerKey)
        {
            BpuIdentifier = new BpuIdentifier(requestHandlerKey);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Handles request
        /// </summary>
        /// <param name="requestTransitionContext">Request transition context</param>
        /// <returns>Response transition context</returns>
        public virtual async Task<IDataSet?> HandleRequestAsync(IDataSet? requestTransitionContext)
        {
            return requestTransitionContext;
        }

        /// <summary>
        /// Initializes component
        /// IBpuaService interface implementation
        /// </summary>
        /// <param name="bppApplication">BPUA application</param>
        public virtual async Task InitializeComponent(IBpuaApplication bppApplication)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets BPUA application
        /// IRequestHandler interface implementation
        /// </summary>
        public IBpuaApplication BpuaApplication
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets BPU identifier
        /// IRequestHandler interface implementation
        /// </summary>
        public IBpuIdentifier BpuIdentifier
        {
            get; private set;
        } = new BpuIdentifier();

        /// <summary>
        /// Gets or sets transition context
        /// IRequestHandler interface implementation
        /// </summary>
        public IDataSet? TransitionContext
        {
            get; set;
        }

        /// <summary>
        /// Gets component identifier
        /// </summary>
        public virtual string ComponentIdentifier
        {
            get
            {
                return KeyCompiler.CompileRequestHandlerKey(BpuIdentifier.DomainName, BpuIdentifier.UseCaseName, BpuIdentifier.ApplicationLayerName, BpuIdentifier.StateName, BpuIdentifier.TransitionName);
            }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Finalizes the processing of the transition context
        /// </summary>
        protected virtual void FinalizeTransitionContextProcessing()
        {
        }

        /// <summary>
        /// Processes request
        /// </summary>
        protected virtual void ProcessRequest()
        {
        }

        /// <summary>
        /// Processes request asynchronously
        /// </summary>
        protected virtual async Task ProcessRequestAsync()
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Processes response
        /// </summary>
        protected virtual void ProcessResponse()
        {
        }

        /// <summary>
        /// Processes response asynchronously
        /// </summary>
        protected virtual async Task ProcessResponseAsync()
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Raises service request event
        /// </summary>
        /// <param name="requestName">Request name</param>
        protected async virtual Task RaiseServiceRequestEventAsync(string requestName)
        {
            if (ServiceRequestEvent == null)
            {
                return;
            }

            IRequestMetadata? requestMetadata = RequestTransitionContext.GetCurrentRequestMetadata();
            if (requestMetadata == null)
            {
                throw new InvalidOperationException("Request metadata is not found in the transition context.");
            }

            requestMetadata.RequestName = requestName;
            RouteTransitionContextEventArgs routeTransitionContextEventArgs = new RouteTransitionContextEventArgs(RequestTransitionContext);

            ServiceRequestEventArgs serviceRequestEventArgs = new ServiceRequestEventArgs(routeTransitionContextEventArgs);
            await ServiceRequestEvent.Invoke(this, serviceRequestEventArgs);

            ResponseTransitionContext = routeTransitionContextEventArgs.TransitionContext;
        }

        /// <summary>
        /// Sends request to application next layer
        /// </summary>
        protected virtual async Task SendRequestToApplicationNextLayer()
        {
            await RaiseServiceRequestEventAsync(BPUA.Application.Contracts.RequestNames.SEND_REQUEST_TO_APPLICATION_NEXT_LAYER);
        }

        /// <summary>
        /// Sends request to next handler
        /// </summary>
        protected virtual async Task SendRequestToNextHandler()
        {
            await RaiseServiceRequestEventAsync(BPUA.Application.Contracts.RequestNames.SEND_REQUEST_TO_NEXT_HANDLER);
        }
        #endregion

        #region Protected Properties
        /// <summary>
        /// Gets or sets request transition context
        /// </summary>
        protected IDataSet? RequestTransitionContext
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets response transition context
        /// </summary>
        protected IDataSet? ResponseTransitionContext
        {
            get; set;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles event asynchronously
        /// IRequestHandler interface implementation
        /// </summary>
        /// <param name="sender">Event source</param>
        /// <param name="args">Event arguments</param>
        public virtual async Task HandleAsync(object? sender, EventArgs args)
        {
            await Task.Delay(0);
        }
        #endregion

        #region Destructors
        /// <summary>
        /// Releases resources
        /// </summary>
        protected override void ReleaseResources()
        {
            ServiceRequestEvent = null;
            BpuaApplication = default!;
            TransitionContext = null;
        }
        #endregion
    }
}
