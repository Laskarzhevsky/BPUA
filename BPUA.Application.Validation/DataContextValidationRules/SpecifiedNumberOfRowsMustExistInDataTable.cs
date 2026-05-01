using BPUA.Application.Contracts;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.IData;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Verifies that specified number of rows must exist in data table.
    /// </summary>
    public class SpecifiedNumberOfRowsMustExistInDataTable : ValidationRule
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataTableName">Data table name</param>
        /// <param name="requestHandlerTypeFullName">Request handler type full name</param>
        /// <param name="expectedRowCount">Expected number of rows in the data table</param>
        public SpecifiedNumberOfRowsMustExistInDataTable(string dataTableName, string requestHandlerTypeFullName, int expectedRowCount) : base(dataTableName, requestHandlerTypeFullName)
        {
            ExpectedRowCount = expectedRowCount;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Validates data context against the rule.
        /// IValidationRule interface implementation
        /// </summary>
        /// <param name="dataContext">The data context to validate.</param>
        public override void Validate(IDataSet? dataContext)
        {
            if (dataContext == null)
            {
                return;
            }

            if (!dataContext.Tables.ContainsKey(DataTableName))
            {
                dataContext.AddMessage(MessageType.Error, $"Data table {DataTableName} is not found at transition handler {RequestHandlerTypeFullName}");
            }

            if (dataContext.Tables[DataTableName].Rows.Count != ExpectedRowCount)
            {
                dataContext.AddMessage(MessageType.Error, $"Data table {DataTableName} must have {ExpectedRowCount} row(s) at transition handler {RequestHandlerTypeFullName}");
            }
        }
        #endregion

        #region Private Properties
        /// <summary>
        /// Gets or sets expected data transfer object count
        /// </summary>
        int ExpectedRowCount
        {
            get; set;
        }
        #endregion
    }
}
