using BPUA.Application.Validation.Contracts;
using PocoDataSet.IData;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Base class for validation rules.
    /// </summary>
    public abstract class BaseValidationRule : IValidationRule
    {
        #region Public Methods
        /// <summary>
        /// Validates component against the rule.
        /// IValidationRule interface implementation.
        /// </summary>
        /// <param name="dataSet">The data set to validate.</param>
        /// <param name="validationResultBuilder">The validation result builder.</param>
        public abstract void Validate(IDataSet? dataSet, IValidationResultBuilder validationResultBuilder);
        #endregion

        #region Protected Methods
        /// <summary>
        /// Adds an error to the validation result builder.
        /// </summary>
        /// <param name="validationResultBuilder">The validation result builder.</param>
        /// <param name="code">The error code.</param>
        /// <param name="message">The error message.</param>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="memberName">The name of the member.</param>
        protected void AddError(IValidationResultBuilder validationResultBuilder, string code, string message, string? tableName = null, string? memberName = null)
        {
            validationResultBuilder.AddIssue(new ValidationIssue(code, message, ValidationIssueSeverity.Error, tableName, memberName));
        }
        #endregion
    }
}
