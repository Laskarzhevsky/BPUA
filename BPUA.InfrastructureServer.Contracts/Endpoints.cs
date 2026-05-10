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
        public static IBpuIdentifier RegisteringHostBpuIdentifier()
        {
            BpuIdentifier bpuIdentifier = new BpuIdentifier();

            bpuIdentifier.RequestName = BPUA.Application.Contracts.RequestNames.ANY;
            bpuIdentifier.DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
            bpuIdentifier.UseCaseName = BPUA.InfrastructureServer.Contracts.UseCaseName.INFRASTRUCTURE_SERVER;
            bpuIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DPL;
            bpuIdentifier.StateName = default!;
            bpuIdentifier.TransitionName = BPUA.InfrastructureServer.Contracts.TransitionsNames.REGISTERING_HOST;

            return bpuIdentifier;
        }
    }
}
