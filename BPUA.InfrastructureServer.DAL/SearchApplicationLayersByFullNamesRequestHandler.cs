using BPUA.Application.Contracts;
using BPUA.Core;

using System.Threading.Tasks;

namespace BPUA.InfrastructureServer.DAL
{
    /// <summary>
    /// RegisteringHost service handler
    /// </summary>
    [RegisterAsBpuaService]
    public partial class SearchApplicationLayersByFullNamesRequestHandler : BPUA.Application.DataAccessLogic.DataAccessLogicRequestHandler
    {
        #region Identification
        /// <summary>
        /// Gets service keys
        /// </summary>
        public static string ServiceKey
        {
            get
            {
                return KeyCompiler.CompileRequestHandlerKey(BPUA.InfrastructureServer.Contracts.DAL.RequestHandlers.BpuIdentifiers.SearchApplicationLayersByFullNames);
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
            await base.ProcessRequestAsync();
        }
        #endregion
    }
}
