using BPUA.Core;

namespace BPUA.InfrastructureServer.Contracts.DAL.RequestHandlers
{
    /// <summary>
    /// Defines SearchApplicationLayersByFullNames BPU identifier 
    /// </summary>
    public static partial class BpuIdentifiers
    {
        #region Public Properties
        /// <summary>
        /// Gets RegisteringHost BPU identifier
        /// </summary>
        public static IBpuIdentifier RegisteringHost
        {
            get 
            {
                BpuIdentifier bpuIdentifier = new BpuIdentifier();

                bpuIdentifier.DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
                bpuIdentifier.UseCaseName = BPUA.InfrastructureServer.Contracts.UseCaseName.INFRASTRUCTURE_SERVER;
                bpuIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DAL;
                bpuIdentifier.TransitionName = BPUA.InfrastructureServer.Contracts.TransitionsNames.REGISTERING_HOST;

                return bpuIdentifier;
            }
        }
        #endregion
    }
}
