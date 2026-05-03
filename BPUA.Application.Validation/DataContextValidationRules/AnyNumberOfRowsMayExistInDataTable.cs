using PocoDataSet.IData;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Verifies that any number of rows may exist in data table.
    /// This rule is used when there is no requirement for the number of rows in the data table, but the data table itself is required.
    /// </summary>
    public class AnyNumberOfRowsMayExistInDataTable : ValidationRule
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataTableName">Data table name</param>
        /// <param name="requestHandlerTypeFullName">Request handler type full name</param>
        public AnyNumberOfRowsMayExistInDataTable(string dataTableName, string requestHandlerTypeFullName) : base(dataTableName, requestHandlerTypeFullName)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Validates data context against the rule.
        /// IValidationRule interface implementation
        /// </summary>
        /// <param name="dataContext">The data context to validate.</param>
        /// <returns>True if the data context is valid; otherwise, false.</returns>
        public override bool Validate(IDataSet? dataContext)
        {
            if (dataContext == null)
            {
                return false;
            }

            DataSetMustContainDataTable dataSetMustContainDataTable = new(DataTableName, RequestHandlerTypeFullName);
            if (!dataSetMustContainDataTable.Validate(dataContext))
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
