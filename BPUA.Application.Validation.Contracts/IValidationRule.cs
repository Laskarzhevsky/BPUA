using PocoDataSet.IData;

namespace BPUA.Application.Validation.Contracts
{
    /// <summary>
    /// Defines a reusable validation rule.
    /// </summary>
    public interface IValidationRule
    {
        #region Methods
        /// <summary>
        /// Validates component against the rule.
        /// </summary>
        /// <param name="dataSet">The data set to validate.</param>
        /// <param name="validationResultBuilder">The validation result builder.</param>
        void Validate(IDataSet? dataSet, IValidationResultBuilder validationResultBuilder);
        #endregion
    }
}
