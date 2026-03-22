using BPUA.Application.Contracts;

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
        /// <param name="transitionName"></param>
        /// <param name="domainName"></param>
        /// <param name="useCaseName"></param>
        /// <param name="applicationLayerName"></param>
        /// <param name="stateName"></param>
        public Transition(string transitionName, string domainName, string useCaseName, string applicationLayerName, string stateName)
        {
            Name = transitionName;
            DomainName = domainName;
            UseCaseName = useCaseName;
            ApplicationLayerName = applicationLayerName;
            SourceStateName = stateName;

            _allowedCallerTypeFullNames = new List<string>();
            InboundDataContract = new TransitionDataContract();
            OutboundDataContract = new TransitionDataContract();
            _targetStateNames = new List<string>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets application layer name
        /// ITransition interface implementation
        /// </summary>
        public string ApplicationLayerName
        {
            get; private set;
        }

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
        /// Gets domain name
        /// ITransition interface implementation
        /// </summary>
        public string DomainName
        {
            get; private set;
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
        /// Gets name
        /// ITransition interface implementation
        /// </summary>
        public string Name
        {
            get; private set;
        }

        /// <summary>
        /// Gets source state name
        /// ITransition interface implementation
        /// </summary>
        public string SourceStateName
        {
            get; private set;
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

        /// <summary>
        /// Gets use case name
        /// ITransition interface implementation
        /// </summary>
        public string UseCaseName
        {
            get; private set;
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



    /// <summary>
    /// Provides transition definition registry extension methods.
    /// </summary>
    public static class TransitionDefinitionRegistryExtensions
    {
        public static Transition RegisterTransition(this ITransitionRegistry transitionDefinitionRegistry, string requestorTypeFullName, string eventName, string? requestedTransitionName, string transitionName, string domainName, string useCaseName, string applicationLayerName, string stateName)
        {
            TransitionDefinitionKey transitionDefinitionKey = new TransitionDefinitionKey( requestorTypeFullName, eventName, requestedTransitionName);

            Transition transitionDefinition = new Transition(transitionName, domainName, useCaseName, applicationLayerName, stateName);
            transitionDefinitionRegistry.RegisterTransition(transitionDefinition);

            return transitionDefinition;
        }

        public static Transition AddInboundTable(this Transition transitionDefinition, string tableName, int minimumRowsCount, int? maximumRowsCount, bool isRequired)
        {
            TransitionDataTableContract transitionDataTableContract =
                new TransitionDataTableContract(
                    tableName,
                    minimumRowsCount,
                    maximumRowsCount,
                    isRequired);

            ((TransitionDataContract)transitionDefinition.InboundDataContract).AddTable(transitionDataTableContract);

            return transitionDefinition;
        }

        public static Transition AddOutboundTable(this Transition transitionDefinition, string tableName, int minimumRowsCount, int? maximumRowsCount, bool isRequired)
        {
            TransitionDataTableContract transitionDataTableContract =
                new TransitionDataTableContract(
                    tableName,
                    minimumRowsCount,
                    maximumRowsCount,
                    isRequired);

            ((TransitionDataContract)transitionDefinition.OutboundDataContract).AddTable(transitionDataTableContract);

            return transitionDefinition;
        }

        public static Transition AddAllowedCaller(this Transition transitionDefinition, string allowedCallerTypeFullName)
        {
            transitionDefinition.AddAllowedCaller(allowedCallerTypeFullName);
            return transitionDefinition;
        }
    }
}
