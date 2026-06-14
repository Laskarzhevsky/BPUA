using BPUA.Application.Contracts;
using BPUA.Core;

using System.Threading.Tasks;

namespace BPUA.InfrastructureServer.DPL
{
    /// <summary>
    /// RegisteringHost service handler
    /// </summary>
    [RegisterAsBpuaService]
    public partial class SearchApplicationLayersByFullNamesRequestHandler : BPUA.Application.DataProcessingLogic.DataProcessingLogicRequestHandler
    {
        #region Identification
        /// <summary>
        /// Gets service keys
        /// </summary>
        public static string ServiceKey
        {
            get
            {
                return KeyCompiler.CompileRequestHandlerKey(BPUA.InfrastructureServer.Contracts.DPL.RequestHandlers.BpuIdentifiers.SearchingApplicationLayersByFullNames);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public SearchApplicationLayersByFullNamesRequestHandler() : base(ServiceKey)
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
        }
        #endregion
    }
}
