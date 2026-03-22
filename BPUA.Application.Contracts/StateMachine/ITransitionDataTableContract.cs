namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines transition data table contract functionality.
    /// </summary>
    public interface ITransitionDataTableContract
    {
        #region Properties
        /// <summary>
        /// Gets flag indicating whether data table is required
        /// </summary>
        bool IsRequired
        {
            get;
        }

        /// <summary>
        /// Gets the maximum number of rows that data table can contain
        /// </summary>
        int? MaximumRowsCount
        {
            get;
        }

        /// <summary>
        /// Gets the minimum number of rows that data table can contain
        /// </summary>
        int MinimumRowsCount
        {
            get;
        }

        /// <summary>
        /// Gets table name
        /// </summary>
        string TableName
        {
            get;
        }
        #endregion
    }
}
