using BPUA.Application.Contracts;
using BPUA.Application.NonFunctionalContracts;
using BPUA.Core;

using PocoDataSet.IData;
using PocoDataSet.SqlServerDataAdapter;

using System;
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
            if (RequestTransitionContext == null)
            {
                return;
            }

            GetConnectionString();
            SqlDataAdapter adapter = new SqlDataAdapter(ConnectionString);
            Console.WriteLine(typeof(Microsoft.Data.SqlClient.SqlConnection).Assembly.Location);
            Console.WriteLine(typeof(Microsoft.Data.SqlClient.SqlConnection).Assembly.FullName);
            IDataTable hostedApplicationLayerTable = RequestTransitionContext.Tables[typeof(IHostedApplicationLayer).Name];
            Microsoft.Data.SqlClient.SqlParameter hostedApplicationLayerParameter = await adapter.CreateTableValuedParameterAsync("@HostedApplicationLayer", "dbo.HostedApplicationLayer", hostedApplicationLayerTable);
            Microsoft.Data.SqlClient.SqlParameter[] sqlParameters = new Microsoft.Data.SqlClient.SqlParameter[1];
            sqlParameters[0] = hostedApplicationLayerParameter;
            
            await adapter.FillAsync(
                "[HostedApplicationLayer].[FindHostedApplicationLayersByIdentifiers]",
                false,
                sqlParameters,
                null,
                null,
                ResponseTransitionContext);
        }
        #endregion
    }
}
