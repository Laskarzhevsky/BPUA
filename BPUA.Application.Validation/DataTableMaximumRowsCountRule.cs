using BPUA.Application.Validation.Contracts;
using PocoDataSet.IData;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Validates maximum rows count for a table.
    /// </summary>
    public class DataTableMaximumRowsCountRule : BaseValidationRule
    {
        #region Fields
        /// <summary>
        /// Holds maximum rows count for a table.
        /// </summary>
        private readonly int _maximumRowsCount;

        /// <summary>
        /// Holds the name of the table.
        /// </summary>
        private readonly string _tableName;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="maximumRowsCount">The maximum number of rows allowed.</param>
        public DataTableMaximumRowsCountRule(string tableName, int maximumRowsCount)
        {
            _tableName = tableName;
            _maximumRowsCount = maximumRowsCount;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Validates component against the rule.
        /// IValidationRule interface implementation.
        /// </summary>
        /// <param name="dataSet">The data set to validate.</param>
        /// <param name="validationResultBuilder">The validation result builder.</param>
        public override void Validate(IDataSet? dataSet, IValidationResultBuilder validationResultBuilder)
        {
            if (dataSet == null)
            {
                return;
            }

            IDataTable? dataTable = dataSet[_tableName];
            if (dataTable == null)
            {
                return;
            }

            if (dataTable.Rows.Count > _maximumRowsCount)
            {
                AddError(validationResultBuilder, "MaximumRowsCountExceeded", "Table contains more rows than allowed.", _tableName);
            }
        }
        #endregion
    }
}
