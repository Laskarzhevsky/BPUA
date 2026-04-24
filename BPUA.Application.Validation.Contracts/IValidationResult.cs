using System.Collections.Generic;

namespace BPUA.Application.Validation.Contracts
{
    /// <summary>
    /// Defines validation result.
    /// </summary>
    public interface IValidationResult
    {
        #region Properties
        /// <summary>
        /// Gets validation issues.
        /// </summary>
        IReadOnlyList<IValidationIssue> Issues
        {
            get;
        }

        /// <summary>
        /// Gets flag indicating whether validation succeeded.
        /// </summary>
        bool IsValid
        {
            get;
        }
        #endregion
    }
}
