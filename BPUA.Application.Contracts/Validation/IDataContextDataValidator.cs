using PocoDataSet.IData;

using System.Collections.Generic;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines validation functionality of the data of the data context
    /// </summary>
    public interface IDataContextDataValidator
    {
        #region Methods
        /// <summary>
        /// Validates the specified data context using the provided validation rules.
        /// </summary>
        /// <param name="dataContext">The data context to validate.</param>
        /// <param name="validationRules">The validation rules to apply.</param>
        void Validate(IDataSet? dataContext, IReadOnlyList<IValidationRule> validationRules);
        #endregion
    }
}
