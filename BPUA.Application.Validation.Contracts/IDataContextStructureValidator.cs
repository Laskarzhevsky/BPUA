using BPUA.Application.Contracts;
using PocoDataSet.IData;

namespace BPUA.Application.Validation.Contracts
{
    /// <summary>
    /// Defines validation functionality of the structure of the data context
    /// </summary>
    public interface IDataContextStructureValidator
    {
        #region Methods
        /// <summary>
        /// Validates data context against transition data contract.
        /// </summary>
        /// <param name="dataContext">The data context to validate.</param>
        /// <param name="transitionDataContract">The transition data contract.</param>
        /// <returns>The validation result.</returns>
        IValidationResult Validate(IDataSet? dataContext, ITransitionDataContract transitionDataContract);
        #endregion
    }
}
