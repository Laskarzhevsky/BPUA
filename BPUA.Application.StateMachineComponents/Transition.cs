using System;
using System.Collections.Generic;

namespace BPUA.Application.StateMachineComponents
{
    /// <summary>
    /// Defines transition selection key functionality.
    /// The key identifies a transition definition that can be selected by the router.
    /// </summary>
    public interface ITransitionDefinitionKey
    {
        string RequestorTypeFullName { get; }
        string EventName { get; }
        string? RequestedTransitionName { get; }
    }

    /// <summary>
    /// Provides transition selection key functionality.
    /// </summary>
    public class TransitionDefinitionKey : ITransitionDefinitionKey
    {
        public TransitionDefinitionKey(
            string requestorTypeFullName,
            string eventName,
            string? requestedTransitionName = null)
        {
            RequestorTypeFullName = requestorTypeFullName;
            EventName = eventName;
            RequestedTransitionName = requestedTransitionName;
        }

        public string RequestorTypeFullName { get; private set; }
        public string EventName { get; private set; }
        public string? RequestedTransitionName { get; private set; }
    }

    /// <summary>
    /// Defines transition data table contract functionality.
    /// </summary>
    public interface ITransitionDataTableContract
    {
        string TableName { get; }
        int MinimumRowsCount { get; }
        int? MaximumRowsCount { get; }
        bool IsRequired { get; }
    }

    /// <summary>
    /// Provides transition data table contract functionality.
    /// </summary>
    public class TransitionDataTableContract : ITransitionDataTableContract
    {
        public TransitionDataTableContract(
            string tableName,
            int minimumRowsCount,
            int? maximumRowsCount,
            bool isRequired)
        {
            TableName = tableName;
            MinimumRowsCount = minimumRowsCount;
            MaximumRowsCount = maximumRowsCount;
            IsRequired = isRequired;
        }

        public string TableName { get; private set; }
        public int MinimumRowsCount { get; private set; }
        public int? MaximumRowsCount { get; private set; }
        public bool IsRequired { get; private set; }
    }

    /// <summary>
    /// Defines transition data contract functionality.
    /// </summary>
    public interface ITransitionDataContract
    {
        IReadOnlyList<ITransitionDataTableContract> Tables { get; }
    }

    /// <summary>
    /// Provides transition data contract functionality.
    /// </summary>
    public class TransitionDataContract : ITransitionDataContract
    {
        private readonly List<ITransitionDataTableContract> _tables;

        public TransitionDataContract()
        {
            _tables = new List<ITransitionDataTableContract>();
        }

        public IReadOnlyList<ITransitionDataTableContract> Tables
        {
            get { return _tables; }
        }

        public void AddTable(ITransitionDataTableContract tableContract)
        {
            if (tableContract == null)
            {
                return;
            }

            _tables.Add(tableContract);
        }
    }

    /// <summary>
    /// Defines transition definition functionality.
    /// </summary>
    public interface ITransitionDefinition
    {
        ITransitionDefinitionKey Key { get; }
        string TransitionName { get; }
        string DomainName { get; }
        string UseCaseName { get; }
        string ApplicationLayerName { get; }
        string StateName { get; }
        IReadOnlyList<string> AllowedCallerTypeFullNames { get; }
        ITransitionDataContract InboundDataContract { get; }
        ITransitionDataContract OutboundDataContract { get; }
    }

    /// <summary>
    /// Provides transition definition functionality.
    /// </summary>
    public class TransitionDefinition : ITransitionDefinition
    {
        private readonly List<string> _allowedCallerTypeFullNames;

        public TransitionDefinition(
            ITransitionDefinitionKey key,
            string transitionName,
            string domainName,
            string useCaseName,
            string applicationLayerName,
            string stateName)
        {
            Key = key;
            TransitionName = transitionName;
            DomainName = domainName;
            UseCaseName = useCaseName;
            ApplicationLayerName = applicationLayerName;
            StateName = stateName;

            _allowedCallerTypeFullNames = new List<string>();
            InboundDataContract = new TransitionDataContract();
            OutboundDataContract = new TransitionDataContract();
        }

        public ITransitionDefinitionKey Key { get; private set; }
        public string TransitionName { get; private set; }
        public string DomainName { get; private set; }
        public string UseCaseName { get; private set; }
        public string ApplicationLayerName { get; private set; }
        public string StateName { get; private set; }

        public IReadOnlyList<string> AllowedCallerTypeFullNames
        {
            get { return _allowedCallerTypeFullNames; }
        }

        public ITransitionDataContract InboundDataContract { get; private set; }
        public ITransitionDataContract OutboundDataContract { get; private set; }

        public void AddAllowedCaller(string allowedCallerTypeFullName)
        {
            if (string.IsNullOrWhiteSpace(allowedCallerTypeFullName))
            {
                return;
            }

            _allowedCallerTypeFullNames.Add(allowedCallerTypeFullName);
        }

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
    }

    /// <summary>
    /// Defines transition definition registry functionality.
    /// </summary>
    public interface ITransitionDefinitionRegistry
    {
        void RegisterTransition(ITransitionDefinition transitionDefinition);

        ITransitionDefinition? GetTransition(
            string requestorTypeFullName,
            string eventName,
            string? requestedTransitionName);
    }

    /// <summary>
    /// Provides transition definition registry functionality.
    /// </summary>
    public class TransitionDefinitionRegistry : ITransitionDefinitionRegistry
    {
        private readonly List<ITransitionDefinition> _transitionDefinitions;

        public TransitionDefinitionRegistry()
        {
            _transitionDefinitions = new List<ITransitionDefinition>();
        }

        public void RegisterTransition(ITransitionDefinition transitionDefinition)
        {
            if (transitionDefinition == null)
            {
                return;
            }

            _transitionDefinitions.Add(transitionDefinition);
        }

        public ITransitionDefinition? GetTransition(
            string requestorTypeFullName,
            string eventName,
            string? requestedTransitionName)
        {
            ITransitionDefinition? transitionDefinition = null;
            int i = 0;

            for (i = 0; i < _transitionDefinitions.Count; i++)
            {
                transitionDefinition = _transitionDefinitions[i];

                if (!string.Equals(
                    transitionDefinition.Key.RequestorTypeFullName,
                    requestorTypeFullName,
                    StringComparison.Ordinal))
                {
                    continue;
                }

                if (!string.Equals(
                    transitionDefinition.Key.EventName,
                    eventName,
                    StringComparison.Ordinal))
                {
                    continue;
                }

                if (!string.Equals(
                    transitionDefinition.Key.RequestedTransitionName,
                    requestedTransitionName,
                    StringComparison.Ordinal))
                {
                    continue;
                }

                return transitionDefinition;
            }

            return null;
        }
    }

    /// <summary>
    /// Provides transition definition registry extension methods.
    /// </summary>
    public static class TransitionDefinitionRegistryExtensions
    {
        public static TransitionDefinition RegisterTransition(
            this ITransitionDefinitionRegistry transitionDefinitionRegistry,
            string requestorTypeFullName,
            string eventName,
            string? requestedTransitionName,
            string transitionName,
            string domainName,
            string useCaseName,
            string applicationLayerName,
            string stateName)
        {
            TransitionDefinitionKey transitionDefinitionKey =
                new TransitionDefinitionKey(
                    requestorTypeFullName,
                    eventName,
                    requestedTransitionName);

            TransitionDefinition transitionDefinition =
                new TransitionDefinition(
                    transitionDefinitionKey,
                    transitionName,
                    domainName,
                    useCaseName,
                    applicationLayerName,
                    stateName);

            transitionDefinitionRegistry.RegisterTransition(transitionDefinition);

            return transitionDefinition;
        }

        public static TransitionDefinition AddInboundTable(
            this TransitionDefinition transitionDefinition,
            string tableName,
            int minimumRowsCount,
            int? maximumRowsCount,
            bool isRequired)
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

        public static TransitionDefinition AddOutboundTable(
            this TransitionDefinition transitionDefinition,
            string tableName,
            int minimumRowsCount,
            int? maximumRowsCount,
            bool isRequired)
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

        public static TransitionDefinition AddAllowedCaller(
            this TransitionDefinition transitionDefinition,
            string allowedCallerTypeFullName)
        {
            transitionDefinition.AddAllowedCaller(allowedCallerTypeFullName);
            return transitionDefinition;
        }
    }
}
