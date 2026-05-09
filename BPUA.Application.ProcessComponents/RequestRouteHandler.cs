using BPUA.Application.Contracts;
using BPUA.Core;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.IData;

using System;
using System.Collections.Generic;

namespace BPUA.Application.ProcessComponents
{

    /// <summary>
    /// Provides request route handler functionality.
    /// </summary>
    public abstract class RequestRouteHandler : IRequestRoute
    {
        #region Data Fields
        /// <summary>
        /// AllowedCallerTypeFullNames property data filed
        /// </summary>
        private readonly List<string> _allowedCallerBpuIdentifiers;

        /// <summary>
        /// AllowedCallerTypeFullNames property data filed
        /// </summary>
        private readonly List<string> _targetStateNames;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="requestName">Request name</param>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">Transition name</param>
        public RequestRouteHandler(string requestName, string domainName, string useCaseName, string applicationLayerName, string stateName, string transitionName)
        {
            BpuIdentifier.RequestName = requestName;
            BpuIdentifier.DomainName = domainName;
            BpuIdentifier.UseCaseName = useCaseName;
            BpuIdentifier.ApplicationLayerName = applicationLayerName;
            BpuIdentifier.StateName = stateName;
            BpuIdentifier.TransitionName = transitionName;

            _allowedCallerBpuIdentifiers = new List<string>();
            _targetStateNames = new List<string>();

            AddRequestDataContextValidationRules();
            AddResponseDataContextValidationRules();
        }

        /// <summary>
        /// Constructor overload that accepts endpoint identifier.
        /// This allows endpoint contracts to be the single source of truth.
        /// </summary>
        public RequestRouteHandler(string requestName, IBpuIdentifier bpuIdentifier)
        {
            BpuIdentifier.RequestName = requestName;
            BpuIdentifier.DomainName = bpuIdentifier.DomainName;
            BpuIdentifier.UseCaseName = bpuIdentifier.UseCaseName;
            BpuIdentifier.ApplicationLayerName = bpuIdentifier.ApplicationLayerName;
            BpuIdentifier.StateName = bpuIdentifier.StateName;
            BpuIdentifier.TransitionName = bpuIdentifier.TransitionName;

            _allowedCallerBpuIdentifiers = new List<string>();
            _targetStateNames = new List<string>();

            AddRequestDataContextValidationRules();
            AddResponseDataContextValidationRules();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets allowed caller type full names
        /// ITransition interface implementation
        /// </summary>
        public IReadOnlyList<string> AllowedCallerTypeFullNames
        {
            get
            {
                return _allowedCallerBpuIdentifiers;
            }
        }

        /// <summary>
        /// Gets BPU identifier
        /// ITransition interface implementation
        /// </summary>
        public IBpuIdentifier BpuIdentifier
        {
            get; private set;
        } = new BpuIdentifier();

        /// <summary>
        /// Gets component identifier
        /// ITransition interface implementation
        /// </summary>
        public string ComponentIdentifier
        {
            get
            {
                return KeyCompiler.CompileRequestRouteKey(BpuIdentifier.RequestName, BpuIdentifier.DomainName, BpuIdentifier.UseCaseName, BpuIdentifier.ApplicationLayerName, BpuIdentifier.StateName, BpuIdentifier.TransitionName);
            }
        }

        /// <summary>
        /// Gets or sets request data context validation rules
        /// ITransition interface implementation
        /// </summary>
        public DistinctList<IValidationRule> RequestDataContextValidationRules
        {
            get; set;
        } = new DistinctList<IValidationRule>();

        /// <summary>
        /// Gets or sets response data context validation rules
        /// ITransition interface implementation
        /// </summary>
        public DistinctList<IValidationRule> ResponseDataContextValidationRules
        {
            get; set;
        } = new DistinctList<IValidationRule>();

        /// <summary>
        /// Gets target state names
        /// ITransition interface implementation
        /// </summary>
        public IReadOnlyList<string> TargetStateNames
        {
            get
            {
                return _targetStateNames;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Processes the request transition context
        /// ITransition interface implementation
        /// </summary>
        /// <param name="requestTransitionContext">Request transition context</param>
        public virtual void ProcessRequestTransitionContext(IDataSet requestTransitionContext)
        {
            IBpuIdentifier? bpuIdentifier = requestTransitionContext.GetCurrentBpuIdentifier();
            if (bpuIdentifier == null)
            {
                throw new System.Exception("BPU identifier metadata is missing in data set.");
            }

            if (!ValidateRequestTransitionContext(requestTransitionContext))
            {
                return;
            }

            if (!ValidateCallerPermission(requestTransitionContext, bpuIdentifier))
            {
                return;
            }

            IBpuIdentifier nextRequestHandlerBpuIdentifier = bpuIdentifier.Clone()!;
            PrepareNextRequestHandlerBpuIdentifier(nextRequestHandlerBpuIdentifier);
            requestTransitionContext.AddRequestMetadata(nextRequestHandlerBpuIdentifier);
        }

        /// <summary>
        /// Processes the response transition context
        /// ITransition interface implementation
        /// </summary>
        /// <param name="responseTransitionContext">Response transition context</param>
        public virtual void ProcessResponseTransitionContext(IDataSet responseTransitionContext)
        {
            responseTransitionContext.RemoveLastRequestMetadata();
            ValidateResponseTransitionContext(responseTransitionContext);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Adds allowed caller
        /// </summary>
        /// <param name="allowedCallerTypeFullName">Allowed caller type full name</param>
        protected void AddAllowedCaller(string allowedCallerTypeFullName)
        {
            if (string.IsNullOrWhiteSpace(allowedCallerTypeFullName))
            {
                return;
            }

            _allowedCallerBpuIdentifiers.Add(allowedCallerTypeFullName);
        }

        /// <summary>
        /// Adds request data context validation rules
        /// </summary>
        protected virtual void AddRequestDataContextValidationRules()
        {
        }

        /// <summary>
        /// Adds response data context validation rules
        /// </summary>
        protected virtual void AddResponseDataContextValidationRules()
        {
        }

        /// <summary>
        /// Adds target state name
        /// </summary>
        /// <param name="targetStateName">Target state name</param>
        protected void AddTargetStateName(string targetStateName)
        {
            _targetStateNames.Add(targetStateName);
        }

        /// <summary>
        /// Prepares the BPU identifier for the next request handler
        /// </summary>
        /// <param name= "nextRequestHandlerBpuIdentifier" >Next request handler BPU identifier</param>
        protected virtual void PrepareNextRequestHandlerBpuIdentifier(IBpuIdentifier nextRequestHandlerBpuIdentifier)
        {
        }

        /// <summary>
        /// Validates caller permission
        /// </summary>
        /// <param name="requestTransitionContext">Request transition context</param>
        /// <param name="currentBpuIdentifier">Current BPU identifier</param>
        /// <returns>True if the data context is valid; otherwise, false.</returns>
        protected bool ValidateCallerPermission(IDataSet requestTransitionContext, IBpuIdentifier currentBpuIdentifier)
        {
            IBpuIdentifier? bpuIdentifier = requestTransitionContext.GetCallerBpuIdentifier();
            if (bpuIdentifier == null)
            {
                throw new System.Exception("BPU identifier of caller is missing in data set.");
            }

            if (_allowedCallerBpuIdentifiers.Count == 0)
            {
                return true;
            }

            string callerBpuIdentifierKey = bpuIdentifier.ToString()!;
            for (int i = 0; i < _allowedCallerBpuIdentifiers.Count; i++)
            {
                if (string.Equals(_allowedCallerBpuIdentifiers[i], callerBpuIdentifierKey, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            requestTransitionContext.AddMessage(MessageType.Error, $"CallerNotAllowed_{callerBpuIdentifierKey}", $"Caller {callerBpuIdentifierKey} is not allowed to execute transition {currentBpuIdentifier.ToString()!}");
            return false;
        }

        /// <summary>
        /// Validates request transition context
        /// </summary>
        /// <param name="requestTransitionContext">Request transition context</param>
        /// <returns>True if the data context is valid; otherwise, false.</returns>
        protected bool ValidateRequestTransitionContext(IDataSet requestTransitionContext)
        {
            foreach (IValidationRule validationRule in RequestDataContextValidationRules)
            {
                if (!validationRule.Validate(requestTransitionContext))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validates response transition context
        /// </summary>
        /// <param name="responseTransitionContext">Response transition context</param>
        /// <returns>True if the data context is valid; otherwise, false.</returns>
        protected bool ValidateResponseTransitionContext(IDataSet responseTransitionContext)
        {
            foreach (IValidationRule validationRule in ResponseDataContextValidationRules)
            {
                if (!validationRule.Validate(responseTransitionContext))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
