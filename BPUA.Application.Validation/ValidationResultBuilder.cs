using BPUA.Application.Validation.Contracts;
using System.Collections.Generic;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Default validation result builder.
    /// </summary>
    public class ValidationResultBuilder : IValidationResultBuilder
    {
        #region Fields
        private readonly List<IValidationIssue> _issues = new List<IValidationIssue>();
        #endregion

        #region Methods
        /// <summary>
        /// Adds validation issue.
        /// </summary>
        public void AddIssue(IValidationIssue issue)
        {
            _issues.Add(issue);
        }

        /// <summary>
        /// Builds validation result.
        /// </summary>
        public IValidationResult Build()
        {
            return new ValidationResult(_issues.AsReadOnly());
        }
        #endregion
    }
}
