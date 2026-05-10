using BPUA.Application.Contracts;
using BPUA.Core;

using System.Threading.Tasks;

namespace BPUA.InfrastructureServer.DPL
{
    /// <summary>
    /// RegisteringHost service handler
    /// </summary>
    [RegisterAsBpuaService]
    public partial class RegisteringHostRequestHandler : BPUA.Application.DataProcessingLogic.DataProcessingLogicRequestHandler
    {
        #region Identification
        /// <summary>
        /// Gets service keys
        /// </summary>
        public static string ServiceKey
        {
            get
            {
                return KeyCompiler.CompileRequestHandlerKey(BPUA.InfrastructureServer.Contracts.DPL.BpuIdentifiers.RegisteringHost);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public RegisteringHostRequestHandler() : base(BPUA.InfrastructureServer.Contracts.DPL.BpuIdentifiers.RegisteringHost)
        {
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Processes request asynchronously
        /// </summary>
        protected override async Task ProcessRequestAsync()
        {
            await SearchApplicationLayersByFullNames();
            MergeApplicationLayersFromRequestWithFound();
            await SaveMergedApplicationLayers();
        }
        #endregion
    }
}
