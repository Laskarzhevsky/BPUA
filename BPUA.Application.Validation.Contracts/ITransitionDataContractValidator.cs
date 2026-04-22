using BPUA.Application.Contracts;
using PocoDataSet.IData;

namespace BPUA.Application.Validation.Contracts
{
    /// <summary>
    /// Defines validation of data set against transition data contract.
    /// </summary>
    public interface ITransitionDataContractValidator
    {
        #region Methods
        /// <summary>
        /// Validates data set against transition data contract.
        /// </summary>
        /// <param name="transitionDataContract">The transition data contract.</param>
        /// <param name="dataSet">The data set to validate.</param>
        /// <returns>The validation result.</returns>
        IValidationResult Validate(ITransitionDataContract transitionDataContract, IDataSet? dataSet);
        #endregion
    }
}
