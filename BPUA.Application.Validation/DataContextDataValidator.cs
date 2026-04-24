using BPUA.Application.Validation.Contracts;

using PocoDataSet.IData;

using System.Collections.Generic;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Provides data validation functionality of a data context using validation rules
    /// </summary>
    public class DataContextDataValidator : IDataContextDataValidator
    {
        #region Methods
        /// <summary>
        /// Validates the specified data context using the provided validation rules.
        /// </summary>
        /// <param name="dataContext">The data context to validate.</param>
        /// <param name="validationRules">The validation rules to apply.</param>
        public IValidationResult Validate(IDataSet? dataContext, IReadOnlyList<IValidationRule> validationRules)
        {
            IValidationResult validationResult = new ValidationResult();
            if (dataContext == null)
            {
                return validationResult;
            }

            for (int index = 0; index < validationRules.Count; index++)
            {
                validationRules[index].Validate(dataContext, validationResult);
            }

            return validationResult;
        }
        #endregion
    }
}
