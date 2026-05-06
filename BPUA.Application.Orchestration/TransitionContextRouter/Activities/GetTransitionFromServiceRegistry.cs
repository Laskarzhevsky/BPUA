using System;

using BPUA.Application.Contracts;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Provides request to the next layer event handler functionality
    /// </summary>
    public partial class TransitionContextRouter
    {
        #region Private Methods
        /// <summary>
        /// Gets transition from service registry
        /// </summary>
        void GetTransitionFromServiceRegistry()
        {
            Type? transitionType = null;
            BpuaApplication!.ServiceRegistry.TryGetRegisteredTransitionType(BpuIdentifier, out transitionType);
            if (transitionType == null)
            {
                throw new ApplicationException($"Transition is not registered for {BpuIdentifier.RequestName}_{BpuIdentifier.DomainName}_{BpuIdentifier.UseCaseName}_{BpuIdentifier.ApplicationLayerName}_{BpuIdentifier.StateName}_{BpuIdentifier.TransitionName}");
            }

            ITransition? transition = Activator.CreateInstance(transitionType) as ITransition;
            if (transition == null)
            {
                throw new ApplicationException($"Transition {BpuIdentifier.RequestName}_{BpuIdentifier.DomainName}_{BpuIdentifier.UseCaseName}_{BpuIdentifier.ApplicationLayerName}_{BpuIdentifier.StateName}_{BpuIdentifier.TransitionName} could not be instantiated.");
            }

            Transition = transition;
        }
        #endregion
    }
}
