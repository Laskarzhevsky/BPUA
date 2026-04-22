namespace BPUA.Application.Validation.Contracts
{
    /// <summary>
    /// Defines a single validation issue.
    /// </summary>
    public interface IValidationIssue
    {
        #region Properties
        /// <summary>
        /// Gets code.
        /// </summary>
        string Code
        {
            get;
        }

        /// <summary>
        /// Gets message.
        /// </summary>
        string Message
        {
            get;
        }

        /// <summary>
        /// Gets optional member name.
        /// </summary>
        string? MemberName
        {
            get;
        }

        /// <summary>
        /// Gets severity.
        /// </summary>
        ValidationIssueSeverity Severity
        {
            get;
        }

        /// <summary>
        /// Gets optional table name.
        /// </summary>
        string? TableName
        {
            get;
        }
        #endregion
    }
}
