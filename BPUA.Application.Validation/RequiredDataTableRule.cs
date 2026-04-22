using BPUA.Application.Validation.Contracts;
using PocoDataSet.IData;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Validates presence of required table.
    /// </summary>
    public class RequiredDataTableRule : BaseValidationRule
    {
        #region Fields
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
        public RequiredDataTableRule(string tableName)
        {
            _tableName = tableName;
        }
        #endregion

        #region Methods
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
                AddError(validationResultBuilder, "RequiredTableIsMissing", "Required table is missing.", _tableName);
            }
        }
        #endregion
    }
}
