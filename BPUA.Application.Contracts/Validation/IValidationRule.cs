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
        void Validate(IDataSet? dataContext);
        #endregion
    }
}
