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
        string? Code
        {
            get;
        }

        /// <summary>
        /// Gets optional column name.
        /// </summary>
        string? ColumnName
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
        /// Gets row index
        /// </summary>
        int? RowIndex 
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
