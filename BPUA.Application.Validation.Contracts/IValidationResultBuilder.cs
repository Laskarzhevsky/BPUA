namespace BPUA.Application.Validation.Contracts
{
    /// <summary>
    /// Defines the contract for building validation results.
    /// </summary>
    public interface IValidationResultBuilder
    {
        #region Methods
        /// <summary>
        /// Adds validation issue.
        /// </summary>
        /// <param name="validationIssue">The validation issue to add.</param>
        void AddIssue(IValidationIssue validationIssue);
        #endregion
    }
}
