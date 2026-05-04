using BPUA.Core;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Provides request to the next layer event handler functionality
    /// </summary>
    public partial class TransitionContextRouter
    {
        #region Private Methods
        /// <summary>
        /// Checks if the hosted application layer is registered in the service registry
        /// </summary>
        /// <returns>True if the hosted application layer is registered; otherwise, false.</returns>
        bool HostedApplicationLayerRegistered()
        {
            string hostedApplicationLayerKey = KeyCompiler.CompileHostedApplicationLayerKey(BpuaIdentifier.DomainName, BpuaIdentifier.UseCaseName, BpuaIdentifier.ApplicationLayerName);
            object? hostedApplicationLayer;
            bool HostedApplicationLayerRegistered = BpuaApplication!.ServiceRegistry.TryGetRegisteredObject(hostedApplicationLayerKey, out hostedApplicationLayer);

            return HostedApplicationLayerRegistered;
        }
        #endregion
    }
}
