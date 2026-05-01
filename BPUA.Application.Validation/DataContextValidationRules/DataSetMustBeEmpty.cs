using BPUA.Application.Contracts;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.IData;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Verifies that data set is empty.
    /// </summary>
    public class DataSetMustBeEmpty : ValidationRule
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dataTableName">Data table name</param>
        /// <param name="requestHandlerTypeFullName">Request handler type full name</param>
        public DataSetMustBeEmpty(string dataTableName, string requestHandlerTypeFullName) : base(dataTableName, requestHandlerTypeFullName)
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

            if (dataContext.Tables.Count > 0)
            {
                dataContext.AddMessage(MessageType.Error, $"Data set must be empty at transition handler {RequestHandlerTypeFullName}");
            }
        }
        #endregion
    }
}
