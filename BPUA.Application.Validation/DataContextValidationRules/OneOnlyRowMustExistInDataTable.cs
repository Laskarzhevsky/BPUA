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

            if (dataContext.Tables[DataTableName].Rows.Count != 1)
            {
                dataContext.AddMessage(MessageType.Error, $"{this.GetType().Name}_{DataTableName}", $"Data table {DataTableName} must have one row only at transition handler {RequestHandlerTypeFullName}");
                return false;
            }

            return true;
        }
        #endregion
    }
}
