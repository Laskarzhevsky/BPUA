using BPUA.Core;

namespace BPUA.InfrastructureServer.Contracts.DPL.RouteHandlers
{
    /// <summary>
    /// Defines RegisteringHost BPU identifier 
    /// </summary>
    public static partial class BpuIdentifiers
    {
        #region Public Properties
        /// <summary>
        /// Gets SearchApplicationLayersByFullNames BPU identifier
        /// </summary>
        public static IBpuIdentifier SearchApplicationLayersByFullNames
        {
            get 
            {
                BpuIdentifier bpuIdentifier = new BpuIdentifier();

                bpuIdentifier.RequestName = BPUA.InfrastructureServer.Contracts.RequestNames.SEARCH_APPLICATION_LAYERS_BY_FULL_NAMES;
                bpuIdentifier.DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
                bpuIdentifier.UseCaseName = BPUA.InfrastructureServer.Contracts.UseCaseName.INFRASTRUCTURE_SERVER;
                bpuIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DPL;
                bpuIdentifier.TransitionName = BPUA.InfrastructureServer.Contracts.TransitionsNames.REGISTERING_HOST;

                return bpuIdentifier;
            }
        }
        #endregion
    }
}
