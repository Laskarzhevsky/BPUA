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
        private readonly List<IValidationIssue> _issues = new List<IValidationIssue>();
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

        #region Public Methods
        /// <summary>
        /// Adds validation issue to the result.
        /// IValidationResult interface implementation
        /// </summary>
        /// <param name="validationIssue">The validation issue to add.</param>
        public void AddIssue(IValidationIssue validationIssue)
        {
            _issues.Add(validationIssue);
        }
        #endregion
    }
}
