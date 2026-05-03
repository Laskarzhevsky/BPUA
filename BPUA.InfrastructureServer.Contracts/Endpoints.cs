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
        public static IBPUAIdentifier RegisteringHost()
        {
            BPUAIdentifier bpuaIdentifier = new BPUAIdentifier();

            bpuaIdentifier.DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
            bpuaIdentifier.UseCaseName = BPUA.InfrastructureServer.Contracts.UseCaseName.INFRASTRUCTURE_SERVER;
            bpuaIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DPL;
            bpuaIdentifier.TransitionName = BPUA.InfrastructureServer.Contracts.TransitionsNames.REGISTERING_HOST;

            return bpuaIdentifier;
        }
    }
}
