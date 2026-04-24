using BPUA.Application.Validation.Contracts;
using PocoDataSet.IData;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Base class for validation rules.
    /// </summary>
    public abstract class ValidationRule : IValidationRule
    {
        #region Public Methods
        /// <summary>
        /// Validates data context against the rule.
        /// </summary>
        /// <param name="dataContext">The data context to validate.</param>
        /// <param name="validationResult">The validation result to update.</param>
        public abstract void Validate(IDataSet? dataContext, IValidationResult validationResult);
        #endregion
    }
}
