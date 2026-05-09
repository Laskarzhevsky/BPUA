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
        /// Gets request route from service registry
        /// </summary>
        void GetRequestRouteFromServiceRegistry()
        {
            Type? requestRouteType = null;
            BpuaApplication!.ServiceRegistry.TryGetRegisteredRequestRouteType(BpuIdentifier, out requestRouteType);
            if (requestRouteType == null)
            {
                BpuIdentifier.RequestName = BPUA.Application.Contracts.RequestNames.ANY;
                BpuaApplication!.ServiceRegistry.TryGetRegisteredRequestRouteType(BpuIdentifier, out requestRouteType);
            }

            if (requestRouteType == null)
            {
                throw new ApplicationException($"Transition is not registered for {BpuIdentifier.RequestName}_{BpuIdentifier.DomainName}_{BpuIdentifier.UseCaseName}_{BpuIdentifier.ApplicationLayerName}_{BpuIdentifier.StateName}_{BpuIdentifier.TransitionName}");
            }

            IRequestRoute? requestRoute = Activator.CreateInstance(requestRouteType) as IRequestRoute;
            if (requestRoute == null)
            {
                throw new ApplicationException($"Transition {BpuIdentifier.RequestName}_{BpuIdentifier.DomainName}_{BpuIdentifier.UseCaseName}_{BpuIdentifier.ApplicationLayerName}_{BpuIdentifier.StateName}_{BpuIdentifier.TransitionName} could not be instantiated.");
            }

            RequestRoute = requestRoute;
        }
        #endregion
    }
}
