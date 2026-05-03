using BPUA.Application.Contracts;
using PocoDataSet.IData;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Base class for validation rules.
    /// </summary>
    public abstract class ValidationRule : IValidationRule
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataTableName">Data table name</param>
        /// <param name="requestHandlerTypeFullName">Request handler type full name</param>
        public ValidationRule(string dataTableName, string requestHandlerTypeFullName)
        {
            DataTableName = dataTableName;
            RequestHandlerTypeFullName = requestHandlerTypeFullName;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Validates data context against the rule.
        /// </summary>
        /// <param name="dataContext">The data context to validate.</param>
        /// <returns>True if the data context is valid; otherwise, false.</returns>
        public abstract bool Validate(IDataSet? dataContext);
        #endregion

        #region Protected Properties
        /// <summary>
        /// Gets or sets data table name
        /// </summary>
        protected string DataTableName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets request handler type full name
        /// </summary>
        protected string RequestHandlerTypeFullName
        {
            get; set;
        }
        #endregion
    }
}
