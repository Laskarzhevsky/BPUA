using PocoDataSet.IData;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines a reusable validation rule.
    /// </summary>
    public interface IValidationRule
    {
        #region Methods
        /// <summary>
        /// Validates data context against the rule.
        /// </summary>
        /// <param name="dataContext">The data context to validate.</param>
        /// <returns>True if the data context is valid; otherwise, false.</returns>
        bool Validate(IDataSet? dataContext);
        #endregion
    }
}
