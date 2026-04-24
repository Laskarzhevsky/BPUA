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
        /// Default constructor
        /// </summary>
        /// <param name="code">The code of the validation issue.</param>
        /// <param name="message">The message of the validation issue.</param>
        /// <param name="severity">The severity of the validation issue.</param>
        /// <param name="tableName">The name of the table associated with the validation issue.</param>
        /// <param name="rowIndex">The index of the row associated with the validation issue.</param>
        /// <param name="columnName">The name of the column associated with the validation issue.</param>
        public ValidationIssue(string? code, string message, ValidationIssueSeverity severity, string? tableName = null, int? rowIndex = null, string? columnName = null)
        {
            Code = code;
            Message = message;
            Severity = severity;
            TableName = tableName;
            RowIndex = rowIndex;
            ColumnName = columnName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets code.
        /// IValidationIssue interface implementation
        /// </summary>
        public string? Code
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets column name.
        /// IValidationIssue interface implementation
        /// </summary>
        public string? ColumnName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets message.
        /// IValidationIssue interface implementation
        /// </summary>
        public string Message
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets row index.
        /// IValidationIssue interface implementation
        /// </summary>
        public int? RowIndex
        {
            get;
            private set;
        } = null;

        /// <summary>
        /// Gets severity.
        /// IValidationIssue interface implementation
        /// </summary>
        public ValidationIssueSeverity Severity
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets table name.
        /// IValidationIssue interface implementation
        /// </summary>
        public string? TableName
        {
            get;
            private set;
        }
        #endregion
    }
}
