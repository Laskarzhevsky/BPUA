using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

using BPUA.Application.Contracts;

using PocoDataSet.IData;

namespace BPUA.Application.StateMachineComponents
{
    public abstract class SearchStateHandler : StateHandler, ISearchStateHandler
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public SearchStateHandler() : base()
        {
        }

        /// <summary>
        /// Creates an instance, taking the state handler identity as arguments
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        public SearchStateHandler(string domainName, string useCaseName, string applicationLayerName, string stateName) : base(domainName, useCaseName, applicationLayerName, stateName)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets metadata
        /// </summary>
        public List<IColumnMetadata> Metadata
        {
            get;
            set;
        } = default!;

        /// <summary>
        /// Gets or sets search criteria schema name
        /// </summary>
        public string SearchCriteriaSchemaName
        {
            get;
            set;
        } = string.Empty;

        /// <summary>
        /// Gets search model
        /// </summary>
        public ExpandoObject SearchModel
        { 
            get;
            set;
        } = default!;
        #endregion

        #region Public Methods
        /// <summary>
        /// Search action
        /// </summary>
        public async Task SearchAsync()
        {
            await Task.CompletedTask;
        }
        #endregion

        #region Destructors
        /// <summary>
        /// Releases resources
        /// </summary>
        protected override void ReleaseResources()
        {
            Metadata = default!;
            SearchModel = default!;

            base.ReleaseResources();
        }
        #endregion
    }
}
