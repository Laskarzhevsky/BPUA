using BPUA.Core;

namespace BPUA.InfrastructureServer.Contracts
{
    /// <summary>
    /// Provides endpoint definitions for the infrastructure server.
    /// </summary>
    public static class Endpoints
    {
        /// <summary>
        /// Provides the identifier for the "Registering Host" endpoint.
        /// </summary>
        /// <returns>Identifier for the "Registering Host" endpoint.</returns>
        public static IBpuIdentifier RegisteringHost()
        {
            BpuIdentifier bpuIdentifier = new BpuIdentifier();

            bpuIdentifier.DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
            bpuIdentifier.UseCaseName = BPUA.InfrastructureServer.Contracts.UseCaseName.INFRASTRUCTURE_SERVER;
            bpuIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DPL;
            bpuIdentifier.TransitionName = BPUA.InfrastructureServer.Contracts.TransitionsNames.REGISTERING_HOST;

            return bpuIdentifier;
        }
    }
}
