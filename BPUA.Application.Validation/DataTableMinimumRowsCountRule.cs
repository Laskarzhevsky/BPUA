using BPUA.Application.Validation.Contracts;
using PocoDataSet.IData;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Validates minimum rows count for a table.
    /// </summary>
    public class DataTableMinimumRowsCountRule : BaseValidationRule
    {
        #region Fields
        /// <summary>
        /// Holds minimum rows count for a table.
        /// </summary>
        private readonly int _minimumRowsCount;

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
        /// <param name="minimumRowsCount">The minimum number of rows required.</param>
        public DataTableMinimumRowsCountRule(string tableName, int minimumRowsCount)
        {
            _tableName = tableName;
            _minimumRowsCount = minimumRowsCount;
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

            if (dataTable.Rows.Count < _minimumRowsCount)
            {
                AddError(validationResultBuilder, "MinimumRowsCountNotReached", "Table contains fewer rows than required.", _tableName);
            }
        }
        #endregion
    }
}
