using PocoDataSet.IData;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Skips data set validation
    /// </summary>
    public class SkipDataSetValidation : ValidationRule
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataTableName">Data table name</param>
        /// <param name="requestHandlerTypeFullName">Request handler type full name</param>
        public SkipDataSetValidation(string dataTableName, string requestHandlerTypeFullName) : base(dataTableName, requestHandlerTypeFullName)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Validates data context against the rule.
        /// IValidationRule interface implementation
        /// </summary>
        /// <returns>True if the data context is valid; otherwise, false.</returns>
        public override bool Validate(IDataSet? dataContext)
        {
            return true;
        }
        #endregion
    }
}
