using BPUA.Application.Contracts;

using System.Collections.Generic;

namespace BPUA.Application.StateMachineComponents
{
    /// <summary>
    /// Provides transition data contract functionality.
    /// </summary>
    public class TransitionDataContract : ITransitionDataContract
    {
        #region DataFields
        /// <summary>
        /// Tables property data field
        /// </summary>
        private readonly List<ITransitionDataTableContract> _tables;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public TransitionDataContract()
        {
            _tables = new List<ITransitionDataTableContract>();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds table
        /// </summary>
        /// <param name="tableContract">Table contract</param>
        public void AddTable(ITransitionDataTableContract tableContract)
        {
            if (tableContract == null)
            {
                return;
            }

            _tables.Add(tableContract);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets tables
        /// ITransitionDataContract interface implementation
        /// </summary>
        public IReadOnlyList<ITransitionDataTableContract> DataTableContracts
        {
            get
            {
                return _tables;
            }
        }
        #endregion
    }
}
