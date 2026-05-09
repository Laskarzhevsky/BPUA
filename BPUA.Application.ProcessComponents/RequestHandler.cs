using BPUA.Application.Contracts;
using BPUA.Core;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.IData;

using System;
using System.Runtime.CompilerServices;
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
        /// Creates an instance, taking the request handler identity as arguments
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">Transition name</param>
        public RequestHandler(string domainName, string useCaseName, string applicationLayerName, string stateName, string? transitionName = null)
        {
            BpuIdentifier.DomainName = domainName;
            BpuIdentifier.UseCaseName = useCaseName;
            BpuIdentifier.ApplicationLayerName = applicationLayerName;
            BpuIdentifier.StateName = stateName;
            BpuIdentifier.TransitionName = transitionName;
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
            RequestTransitionContext = requestTransitionContext;
            ResponseTransitionContext = requestTransitionContext;
            if (requestTransitionContext == null)
            {
                return ResponseTransitionContext;
            }

            ProcessRequest();
            await ProcessRequestAsync();
            if (DoNotSendRequestToApplicationNextLayer)
            {
                FinalizeTransitionContextProcessing();
                return ResponseTransitionContext;
            }

            await SendRequestToApplicationNextLayer();
            if (ResponseTransitionContext == null)
            {
                return ResponseTransitionContext;
            }

            ProcessResponse();
            await ProcessResponseAsync();
            FinalizeTransitionContextProcessing();
            return ResponseTransitionContext;
        }

        /// <summary>
        /// Initializes component
        /// IBpuaService interface implementation
        /// </summary>
        /// <param name="bppApplication">BPUA application</param>
        public virtual async Task InitializeComponent(IBpuaApplication bppApplication)
        {
        }

        /// <summary>
        /// Raises service request event
        /// </summary>
        /// <param name="eventName">Event name</param>
        protected async Task RaiseServiceRequestEventAsync([CallerMemberName] string requestName = "")
        {
            RouteTransitionContextEventArgs routeTransitionContextEventArgs = new RouteTransitionContextEventArgs(RequestTransitionContext);
            await RaiseServiceRequestEventAsync(routeTransitionContextEventArgs, requestName);
            ResponseTransitionContext = routeTransitionContextEventArgs.TransitionContext;
        }

        /// <summary>
        /// Raises service request event
        /// IRequestHandler interface implementation
        /// </summary>
        /// <param name="eventArguments">Event arguments</param>
        /// <param name="eventName">Event name</param>
        public virtual async Task RaiseServiceRequestEventAsync(EventArgs eventArguments, [CallerMemberName] string eventName = "")
        {
            if (ServiceRequestEvent == null)
            {
                return;
            }

            ServiceRequestEventArgs serviceRequestEventArgs = new ServiceRequestEventArgs(eventName, eventArguments);
            await ServiceRequestEvent.Invoke(this, serviceRequestEventArgs);
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
        /// Sends request to application next layer
        /// </summary>
        protected virtual async Task SendRequestToApplicationNextLayer()
        {
            IRequestMetadata? requestMetadata = RequestTransitionContext.GetCurrentRequestMetadata();
            if (requestMetadata == null)
            {
                throw new InvalidOperationException("Request transition context does not contain request metadata");
            }

            RouteTransitionContextEventArgs routeTransitionContextEventArgs = new RouteTransitionContextEventArgs(RequestTransitionContext);
            await RaiseServiceRequestEventAsync(routeTransitionContextEventArgs);

            ResponseTransitionContext = routeTransitionContextEventArgs.TransitionContext;
        }

        /// <summary>
        /// Sends request to next handler
        /// </summary>
        protected virtual async Task SendRequestToNextHandler()
        {
            IRequestMetadata? requestMetadata = RequestTransitionContext.GetCurrentRequestMetadata();
            if (requestMetadata == null)
            {
                throw new InvalidOperationException("Request transition context does not contain request metadata");
            }

            RouteTransitionContextEventArgs routeTransitionContextEventArgs = new RouteTransitionContextEventArgs(RequestTransitionContext);
            await RaiseServiceRequestEventAsync(routeTransitionContextEventArgs);

            ResponseTransitionContext = routeTransitionContextEventArgs.TransitionContext;
        }
        #endregion

        #region Protected Properties
        /// <summary>
        /// Get or sets flag indicating whether request should not be sent to application next layer
        /// </summary>
        protected bool DoNotSendRequestToApplicationNextLayer
        {
            get; set;
        }

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
