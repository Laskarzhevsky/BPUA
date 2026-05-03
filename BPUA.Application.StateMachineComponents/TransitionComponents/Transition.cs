using BPUA.Application.Contracts;
using BPUA.Core;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.IData;

using System;
using System.Collections.Generic;

namespace BPUA.Application.StateMachineComponents
{

    /// <summary>
    /// Provides transition definition functionality.
    /// </summary>
    public abstract class Transition : ITransition
    {
        #region Data Fields
        /// <summary>
        /// AllowedCallerTypeFullNames property data filed
        /// </summary>
        private readonly List<string> _allowedCallerBpuaIdentifiers;

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
        public Transition(string requestName, string domainName, string useCaseName, string applicationLayerName, string stateName, string transitionName)
        {
            BpuaIdentifier.RequestName = requestName;
            BpuaIdentifier.DomainName = domainName;
            BpuaIdentifier.UseCaseName = useCaseName;
            BpuaIdentifier.ApplicationLayerName = applicationLayerName;
            BpuaIdentifier.StateName = stateName;
            BpuaIdentifier.TransitionName = transitionName;

            _allowedCallerBpuaIdentifiers = new List<string>();
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
                return _allowedCallerBpuaIdentifiers;
            }
        }

        /// <summary>
        /// Gets BPUA identifier
        /// IRequestHandler interface implementation
        /// </summary>
        public IBPUAIdentifier BpuaIdentifier
        {
            get; private set;
        } = new BPUAIdentifier();

        /// <summary>
        /// Gets component identifier
        /// IRequestHandler interface implementation
        /// </summary>
        public string ComponentIdentifier
        {
            get
            {
                return KeyCompiler.CompileTransitionKey(BpuaIdentifier.RequestName, BpuaIdentifier.DomainName, BpuaIdentifier.UseCaseName, BpuaIdentifier.ApplicationLayerName, BpuaIdentifier.StateName, BpuaIdentifier.TransitionName);
            }
        }

        /// <summary>
        /// Gets flag indicating whether the transition is an endpoint in the use case.
        /// It can be called from outside of the use case.
        /// IRequestHandler interface implementation
        /// </summary>
        public bool IsEndpoint
        {
            get; protected set;
        }

        /// <summary>
        /// Gets or sets request data context validation rules
        /// IRequestHandler interface implementation
        /// </summary>
        public DistinctList<IValidationRule> RequestDataContextValidationRules
        {
            get; set;
        } = new DistinctList<IValidationRule>();

        /// <summary>
        /// Gets or sets response data context validation rules
        /// IRequestHandler interface implementation
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
            IBPUAIdentifier? bpuaIdentifier = requestTransitionContext.GetCurrentBpuaIdentifier();
            if (bpuaIdentifier == null)
            {
                throw new System.Exception("BPUA identifier metadata is missing in data set.");
            }

            if (!ValidateRequestTransitionContext(requestTransitionContext))
            {
                return;
            }

            if (!ValidateCallerPermission(requestTransitionContext, bpuaIdentifier))
            {
                return;
            }

            IBPUAIdentifier nextTransitionHandlerBpuaIdentifier = bpuaIdentifier.Clone()!;
            PrepareNextTransitionHandlerBpuaIdentifier(nextTransitionHandlerBpuaIdentifier);
            requestTransitionContext.AddRequestMetadata(nextTransitionHandlerBpuaIdentifier);
        }

        /// <summary>
        /// Processes the response transition context
        /// ITransition interface implementation
        /// </summary>
        /// <param name="responseTransitionContext">Response transition context</param>
        public virtual void ProcessResponseTransitionContext(IDataSet responseTransitionContext)
        {
            responseTransitionContext.RemoveLastRequestMetadata();
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

            _allowedCallerBpuaIdentifiers.Add(allowedCallerTypeFullName);
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
        /// Prepares the BPUA identifier for the next transition handler
        /// </summary>
        /// <param name= "nextTransitionHandlerBpuaIdentifier" >Next transition handler BPUA identifier</param>
        protected virtual void PrepareNextTransitionHandlerBpuaIdentifier(IBPUAIdentifier nextTransitionHandlerBpuaIdentifier)
        {
        }

        /// <summary>
        /// Validates caller permission
        /// </summary>
        /// <param name="requestTransitionContext">Request transition context</param>
        /// <param name="currentBpuaIdentifier">Current BPUA identifier</param>
        /// <returns>True if the data context is valid; otherwise, false.</returns>
        protected bool ValidateCallerPermission(IDataSet requestTransitionContext, IBPUAIdentifier currentBpuaIdentifier)
        {
            IBPUAIdentifier? bpuaIdentifier = requestTransitionContext.GetCallerBpuaIdentifier();
            if (bpuaIdentifier == null)
            {
                throw new System.Exception("BPUA identifier of caller is missing in data set.");
            }

            if (_allowedCallerBpuaIdentifiers.Count == 0)
            {
                return true;
            }

            string callerBPUAIdentifierKey = bpuaIdentifier.ToString()!;
            for (int i = 0; i < _allowedCallerBpuaIdentifiers.Count; i++)
            {
                if (string.Equals(_allowedCallerBpuaIdentifiers[i], callerBPUAIdentifierKey, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            requestTransitionContext.AddMessage(MessageType.Error, $"Caller {callerBPUAIdentifierKey} is not allowed to execute transition {currentBpuaIdentifier.ToString()!}");
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
        #endregion
    }
}
