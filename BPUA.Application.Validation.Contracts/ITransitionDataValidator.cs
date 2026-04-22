using PocoDataSet.IData;

namespace BPUA.Application.Validation.Contracts
{
    /// <summary>
    /// Defines a validator for transition data.
    /// </summary>
    public interface ITransitionDataValidator
    {
        #region Methods
        /// <summary>
        /// Validates the transition data set.
        /// </summary>
        /// <param name="dataSet">The data set to validate.</param>
        /// <returns>The validation result.</returns>
        IValidationResult Validate(IDataSet? dataSet);
        #endregion
    }
}
