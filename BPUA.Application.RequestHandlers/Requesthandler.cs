using System;
using System.Threading.Tasks;

using BPUA.Application.Contracts;
using PocoDataSet.BPUAExtensions;
using BPUA.Application.EventArguments;
using BPUA.Core;

using PocoDataSet.Extensions;
using PocoDataSet.IData;

namespace BPUA.Application.RequestHandlers
{
    public abstract class RequestHandler : AsyncDisposableObject, IRequestHandler, IBPUAService
    {
        #region Events
        /// <summary>
        /// Reqests service
        /// IRequestHandler interface implementation
        /// </summary>
        public event Func<object?, EventArgs, Task>? RequestServiceEvent;
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
        public RequestHandler(string domainName, string useCaseName, string applicationLayerName, string stateName)
        {
            DomainNameAtRuntime = domainName;
            UseCaseNameAtRuntime = useCaseName;
            ApplicationLayerNameAtRuntime = applicationLayerName;
            StateNameAtRuntime = stateName;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets new data set
        /// IRequestHandler interface implementation
        /// </summary>
        /// <returns>New data set</returns>
        public virtual IDataSet GetNewDataSet()
        {
            IDataSet dataSet = DataSetFactory.CreateDataSet();
            dataSet.AddRequestMetadataTable();
            return dataSet;
        }

        /// <summary>
        /// Handles request
        /// </summary>
        /// <param name="requestDataSet">Request data set</param>
        /// <returns>Response data set</returns>
        public virtual async Task<IDataSet?> HandleRequestAsync(IDataSet? requestDataSet)
        {
            RequestDataSet = requestDataSet;
            ResponseDataSet = requestDataSet;
            if (requestDataSet == null)
            {
                return ResponseDataSet;
            }

            ProcessRequest();
            if (_terminateNextStepsOfTransitionHandling)
            {
                return ResponseDataSet;
            }

            await SendRequestToApplicationNextLayer();
            if (ResponseDataSet == null)
            {
                return ResponseDataSet;
            }

            ProcessResponse();
            await ProcessResponseAsync();
            return ResponseDataSet;
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
        /// </summary>
        /// <param name="args">Event arguments</param>
        public virtual async Task RaiseServiceRequestEventAsync(EventArgs args)
        {
            if (RequestServiceEvent != null)
            {
                await RequestServiceEvent.Invoke(this, args);
            }
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
        /// Gets or sets application layer name
        /// </summary>
        public string ApplicationLayerNameAtRuntime
        {
            get;
            set;
        } = default!;

        /// <summary>
        /// Gets or sets data set
        /// IRequestHandler interface implementation
        /// </summary>
        public IDataSet? DataSet
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets domain name
        /// </summary>
        public string DomainNameAtRuntime
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets request handler key
        /// </summary>
        public abstract string RequestHandlerKey
        {
            get;
        }

        /// <summary>
        /// Gets or sets state name
        /// </summary>
        public string StateNameAtRuntime
        {
            get;
            set;
        } = default!;

        /// <summary>
        /// Gets or sets application layer name
        /// </summary>
        public string UseCaseNameAtRuntime
        {
            get;
            set;
        } = default!;
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
            RequestToNextLayerEventArgs requestToNextLayerEventArgs = new RequestToNextLayerEventArgs(RequestDataSet);
            await RaiseServiceRequestEventAsync(requestToNextLayerEventArgs);

            ResponseDataSet = requestToNextLayerEventArgs.DataSet;
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
        /// Gets or sets request data set
        /// </summary>
        protected IDataSet? RequestDataSet
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets response data set
        /// </summary>
        protected IDataSet? ResponseDataSet
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
            RequestServiceEvent = null;
            BPUAApplication = default!;
            DataSet = null;
        }
        #endregion
    }
}
