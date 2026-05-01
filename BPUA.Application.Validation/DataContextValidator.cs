using BPUA.Application.Contracts;

using PocoDataSet.IData;

using System.Collections.Generic;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Provides data context validator functionality
    /// </summary>
    public class DataContextValidator : IDataContextValidator
    {
        #region Methods
        /// <summary>
        /// Validates the specified data context using the provided validation rules.
        /// </summary>
        /// <param name="dataContext">The data context to validate.</param>
        /// <param name="validationRules">The validation rules to apply.</param>
        public void Validate(IDataSet? dataContext, IReadOnlyList<IValidationRule> validationRules)
        {
            if (dataContext == null)
            {
                return;
            }

            for (int index = 0; index < validationRules.Count; index++)
            {
                validationRules[index].Validate(dataContext);
            }
        }
        #endregion
    }
}
