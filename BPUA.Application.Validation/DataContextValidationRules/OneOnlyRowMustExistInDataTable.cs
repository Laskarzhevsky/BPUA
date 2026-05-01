using BPUA.Application.Contracts;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.IData;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Verifies that one row only must exist in data table.
    /// </summary>
    public class OneOnlyRowMustExistInDataTable : ValidationRule
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataTableName">Data table name</param>
        /// <param name="requestHandlerTypeFullName">Request handler type full name</param>
        public OneOnlyRowMustExistInDataTable(string dataTableName, string requestHandlerTypeFullName) : base(dataTableName, requestHandlerTypeFullName)
        {
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

            if (dataContext.Tables[DataTableName].Rows.Count != 1)
            {
                dataContext.AddMessage(MessageType.Error, $"Data table {DataTableName} must have one row only at transition handler {RequestHandlerTypeFullName}");
            }
        }
        #endregion
    }
}
