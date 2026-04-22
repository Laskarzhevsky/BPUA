using BPUA.Application.Validation.Contracts;
using System.Collections.Generic;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Default validation result implementation.
    /// </summary>
    public class ValidationResult : IValidationResult
    {
        #region Fields
        private readonly IReadOnlyList<IValidationIssue> _issues;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        public ValidationResult(IReadOnlyList<IValidationIssue> issues)
        {
            _issues = issues;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets validation issues.
        /// </summary>
        public IReadOnlyList<IValidationIssue> Issues
        {
            get
            {
                return _issues;
            }
        }

        /// <summary>
        /// Gets flag indicating whether validation succeeded.
        /// </summary>
        public bool IsValid
        {
            get
            {
                int index;

                for (index = 0; index < _issues.Count; index++)
                {
                    if (_issues[index].Severity == ValidationIssueSeverity.Error)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
        #endregion
    }
}
