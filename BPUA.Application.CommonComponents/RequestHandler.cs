using BPUA.Application.Contracts;
using BPUA.Core;

using PocoDataSet.IData;

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BPUA.Application.CommonComponents
{
    public abstract class RequestHandler : AsyncDisposableObject, IRequestHandler, IBPUAService
    {
        #region Events
        /// <summary>
        /// Reqests service
        /// IRequestHandler interface implementation
        /// </summary>
        public event Func<object?, ServiceRequestEventArgs, Task>? ServiceRequestEvent;
        #endregion

        #region Data Fields
        /// <summary>
        /// Flag indicating whether next steps of transition handling process needs to be terminated
        /// </summary>
        protected bool _terminateNextStepsOfTransitionHandling;
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
            BpuaIdentifier.DomainName = domainName;
            BpuaIdentifier.UseCaseName = useCaseName;
            BpuaIdentifier.ApplicationLayerName = applicationLayerName;
            BpuaIdentifier.StateName = stateName;
            BpuaIdentifier.TransitionName = transitionName;
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
            if (_terminateNextStepsOfTransitionHandling)
            {
                return ResponseTransitionContext;
            }

            await SendRequestToApplicationNextLayer();
            if (ResponseTransitionContext == null)
            {
                return ResponseTransitionContext;
            }

            ProcessResponse();
            await ProcessResponseAsync();
            return ResponseTransitionContext;
        }

        /// <summary>
        /// Initializes component
        /// IBPUAService interface implementation
        /// </summary>
        /// <param name="bppApplication">BPUA application</param>
        public virtual void InitializeComponent(IBPUAApplication bppApplication)
        {
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

            ServiceRequestEventArgs serviceRequestEventArgs = new ServiceRequestEventArgs(GetType(), eventName, eventArguments);
            await ServiceRequestEvent.Invoke(this, serviceRequestEventArgs);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets BPUA application
        /// IRequestHandler interface implementation
        /// </summary>
        public IBPUAApplication BPUAApplication
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets BPUA identifier
        /// IRequestHandler interface implementation
        /// </summary>
        public IBPUAIdentifier BpuaIdentifier
        {
            get; private set;
        } = new BPUAIdentifier();

        /// <summary>
        /// Gets or sets transition context
        /// IRequestHandler interface implementation
        /// </summary>
        public IDataSet? TransitionContext
        {
            get; set;
        }

        /// <summary>
        /// Gets request handler key
        /// </summary>
        public abstract string ComponentIdentifier
        {
            get;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Processes request
        /// </summary>
        protected virtual void ProcessRequest()
        {
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
            RouteTransitionContextEventArgs routeTransitionContextEventArgs = new RouteTransitionContextEventArgs(RequestTransitionContext);
            await RaiseServiceRequestEventAsync(routeTransitionContextEventArgs);

            ResponseTransitionContext = routeTransitionContextEventArgs.TransitionContext;
        }

        /// <summary>
        /// Sets flag indicating whether next steps of transition handling process needs to be terminated
        /// </summary>
        protected void TerminateNextStepsOfTransitionHandling()
        {
            _terminateNextStepsOfTransitionHandling = true;
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
            BPUAApplication = default!;
            TransitionContext = null;
        }
        #endregion
    }
}
