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
            BpuaApplication!.ServiceRegistry.TryGetRegisteredTransitionType(BpuaIdentifier, out transitionType);
            if (transitionType == null)
            {
                throw new ApplicationException($"Transition is not registered for {BpuaIdentifier.RequestName}_{BpuaIdentifier.DomainName}_{BpuaIdentifier.UseCaseName}_{BpuaIdentifier.ApplicationLayerName}_{BpuaIdentifier.StateName}_{BpuaIdentifier.TransitionName}");
            }

            ITransition? transition = Activator.CreateInstance(transitionType) as ITransition;
            if (transition == null)
            {
                throw new ApplicationException($"Transition {BpuaIdentifier.RequestName}_{BpuaIdentifier.DomainName}_{BpuaIdentifier.UseCaseName}_{BpuaIdentifier.ApplicationLayerName}_{BpuaIdentifier.StateName}_{BpuaIdentifier.TransitionName} could not be instantiated.");
            }

            Transition = transition;
        }
        #endregion
    }
}
