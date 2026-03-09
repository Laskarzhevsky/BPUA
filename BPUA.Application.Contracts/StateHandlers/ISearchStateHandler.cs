using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

using PocoDataSet.IData;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defined search state handler functionality
    /// </summary>
    public interface ISearchStateHandler : IStateHandler
    {
        #region Methods
        /// <summary>
        /// Search action
        /// </summary>
        Task SearchAsync();
        #endregion

        #region Properties
        /// <summary>
        /// Gets metadata
        /// </summary>
        List<IColumnMetadata> Metadata
        {
            get;
        }

        /// <summary>
        /// Gets search model
        /// </summary>
        ExpandoObject SearchModel
        {
            get;
        }

        /// <summary>
        /// Gets or sets search criteria schema name
        /// </summary>
        string SearchCriteriaSchemaName
        {
            get;
            set;
        }
        #endregion
    }
}
