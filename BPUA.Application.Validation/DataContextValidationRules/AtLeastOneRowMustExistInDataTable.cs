using BPUA.Application.Contracts;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.IData;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Verifies that at least one row must exist in data table.
    /// </summary>
    public class AtLeastOneRowMustExistInDataTable : ValidationRule
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataTableName">Data table name</param>
        /// <param name="requestHandlerTypeFullName">Request handler type full name</param>
        public AtLeastOneRowMustExistInDataTable(string dataTableName, string requestHandlerTypeFullName) : base(dataTableName, requestHandlerTypeFullName)
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

            if (!dataContext.Tables.ContainsKey(DataTableName))
            {
                dataContext.AddMessage(MessageType.Error, $"Data table {DataTableName} is not found at transition handler {RequestHandlerTypeFullName}");
                return false;
            }

            if (dataContext.Tables[DataTableName].Rows.Count == 0)
            {
                dataContext.AddMessage(MessageType.Error, $"At least one row must exist in data table {DataTableName} at transition handler {RequestHandlerTypeFullName}");
                return false;
            }

            return true;
        }
        #endregion
    }
}
