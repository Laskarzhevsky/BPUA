using BPUA.Core;

namespace BPUA.InfrastructureServer.Contracts.DPL.RequestHandlers
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
        public static IBpuIdentifier SearchingApplicationLayersByFullNames
        {
            get 
            {
                BpuIdentifier bpuIdentifier = new BpuIdentifier();

                bpuIdentifier.DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
                bpuIdentifier.UseCaseName = BPUA.InfrastructureServer.Contracts.UseCaseName.INFRASTRUCTURE_SERVER;
                bpuIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DPL;
                bpuIdentifier.TransitionName = BPUA.InfrastructureServer.Contracts.TransitionsNames.SEARCHING_APPLICATION_LAYERS_BY_FULL_NAMES;

                return bpuIdentifier;
            }
        }
        #endregion
    }
}
