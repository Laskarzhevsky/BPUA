using BPUA.Application.Contracts;
using BPUA.Core;

using System;
using System.Collections.Generic;

namespace BPUA.Application.StateMachineComponents
{

    /// <summary>
    /// Provides transition definition functionality.
    /// </summary>
    public class Transition : ITransition
    {
        #region Data Fields
        /// <summary>
        /// AllowedCallerTypeFullNames property data filed
        /// </summary>
        private readonly List<string> _allowedCallerTypeFullNames;

        /// <summary>
        /// AllowedCallerTypeFullNames property data filed
        /// </summary>
        private readonly List<string> _targetStateNames;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">Transition name</param>
        public Transition(string domainName, string useCaseName, string applicationLayerName, string stateName, string transitionName)
        {
            BpuaIdentifier.DomainName = domainName;
            BpuaIdentifier.UseCaseName = useCaseName;
            BpuaIdentifier.ApplicationLayerName = applicationLayerName;
            BpuaIdentifier.StateName = stateName;
            BpuaIdentifier.TransitionName = transitionName;

            _allowedCallerTypeFullNames = new List<string>();
            InboundDataContract = new TransitionDataContract();
            OutboundDataContract = new TransitionDataContract();
            _targetStateNames = new List<string>();
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
                return _allowedCallerTypeFullNames;
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
        /// </summary>
        public string ComponentIdentifier
        {
            get
            {
                return KeyCompiler.CompileTransitionKey(BpuaIdentifier.DomainName, BpuaIdentifier.UseCaseName, BpuaIdentifier.ApplicationLayerName, BpuaIdentifier.StateName, BpuaIdentifier.TransitionName);
            }
        }

        /// <summary>
        /// Gets inbound data contract
        /// ITransition interface implementation
        /// </summary>
        public ITransitionDataContract InboundDataContract
        {
            get; private set;
        }

        /// <summary>
        /// Gets flag indicating whether this is the default transition which starts from the specified state
        /// ITransition interface implementation
        /// </summary>
        public bool IsDefaultForState
        {
            get; set;
        }

        /// <summary>
        /// Gets outbound data contract
        /// ITransition interface implementation
        /// </summary>
        public ITransitionDataContract OutboundDataContract
        {
            get; private set;
        }

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
        /// Adds allowed caller
        /// </summary>
        /// <param name="allowedCallerTypeFullName">Allowed caller type full name</param>
        public void AddAllowedCaller(string allowedCallerTypeFullName)
        {
            if (string.IsNullOrWhiteSpace(allowedCallerTypeFullName))
            {
                return;
            }

            _allowedCallerTypeFullNames.Add(allowedCallerTypeFullName);
        }

        /// <summary>
        /// Adds target state name
        /// </summary>
        /// <param name="targetStateName">Target state name</param>
        public void AddTargetStateName(string targetStateName)
        {
            _targetStateNames.Add(targetStateName);
        }

        /// <summary>
        /// Gets flag indicating whether caller is allowed
        /// </summary>
        /// <param name="callerTypeFullName">Caller type full name</param>
        /// <returns></returns>
        public bool IsCallerAllowed(string? callerTypeFullName)
        {
            int i = 0;

            if (_allowedCallerTypeFullNames.Count == 0)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(callerTypeFullName))
            {
                return false;
            }

            for (i = 0; i < _allowedCallerTypeFullNames.Count; i++)
            {
                if (string.Equals(_allowedCallerTypeFullNames[i], callerTypeFullName, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
