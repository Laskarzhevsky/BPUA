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
        public static string DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
        public static string UseCaseName = BPUA.InfrastructureServer.Contracts.UseCaseName.INFRASTRUCTURE_SERVER;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DAL;
        public static string StateName = default!;
        public static string TransitionName = BPUA.InfrastructureServer.Contracts.TransitionsNames.SEARCH_APPLICATION_LAYERS_BY_FULL_NAMES;

        /// <summary>
        /// Gets service keys
        /// </summary>
        public static string ServiceKey
        {
            get
            {
                return KeyCompiler.CompileRequestHandlerKey(DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public SearchApplicationLayersByFullNamesRequestHandler() : base(DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName)
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
