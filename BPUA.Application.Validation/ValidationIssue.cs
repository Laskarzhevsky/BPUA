using BPUA.Application.Validation.Contracts;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Default validation issue implementation.
    /// </summary>
    public class ValidationIssue : IValidationIssue
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationIssue"/> class.
        /// </summary>
        public ValidationIssue(string code, string message, ValidationIssueSeverity severity, string? tableName = null, string? memberName = null)
        {
            Code = code;
            Message = message;
            Severity = severity;
            TableName = tableName;
            MemberName = memberName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets code.
        /// </summary>
        public string Code
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets message.
        /// </summary>
        public string Message
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets member name.
        /// </summary>
        public string? MemberName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets severity.
        /// </summary>
        public ValidationIssueSeverity Severity
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets table name.
        /// </summary>
        public string? TableName
        {
            get;
            private set;
        }
        #endregion
    }
}
