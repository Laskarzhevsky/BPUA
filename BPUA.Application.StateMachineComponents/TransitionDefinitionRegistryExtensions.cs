using BPUA.Application.Contracts;
using BPUA.Application.StateMachineComponents;

namespace BPUA.Application
{
    /// <summary>
    /// Provides transition definition registry extension methods.
    /// </summary>
    public static class TransitionDefinitionRegistryExtensions
    {
        public static Transition RegisterTransition(this ITransitionRegistry transitionDefinitionRegistry, string requestorTypeFullName, string eventName, string? requestedTransitionName, string transitionName, string domainName, string useCaseName, string applicationLayerName, string stateName)
        {
            TransitionKey transitionDefinitionKey = new TransitionKey(requestorTypeFullName, eventName, requestedTransitionName);

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
