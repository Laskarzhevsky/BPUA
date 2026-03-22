using BPUA.Application.Contracts;

namespace BPUA.Application
{
    /// <summary>
    /// Provides transition data table contract functionality.
    /// </summary>
    public class TransitionDataTableContract : ITransitionDataTableContract
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="minimumRowsCount">Minimum rows count</param>
        /// <param name="maximumRowsCount">Maximum rows count</param>
        /// <param name="isRequired">Flag indicating whether data table is required</param>
        public TransitionDataTableContract(string tableName, int minimumRowsCount, int? maximumRowsCount, bool isRequired)
        {
            TableName = tableName;
            MinimumRowsCount = minimumRowsCount;
            MaximumRowsCount = maximumRowsCount;
            IsRequired = isRequired;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets flag indicating whether data table is required
        /// ITransitionDataTableContract interface implementation
        /// </summary>
        public bool IsRequired
        {
            get; private set;
        }

        /// <summary>
        /// Gets the maximum number of rows that data table can contain
        /// ITransitionDataTableContract interface implementation
        /// </summary>
        public int? MaximumRowsCount
        {
            get; private set;
        }

        /// <summary>
        /// Gets the minimum number of rows that data table can contain
        /// ITransitionDataTableContract interface implementation
        /// </summary>
        public int MinimumRowsCount
        {
            get; private set;
        }

        /// <summary>
        /// Gets table name
        /// ITransitionDataTableContract interface implementation
        /// </summary>
        public string TableName
        {
            get; private set;
        }
        #endregion
    }
}

