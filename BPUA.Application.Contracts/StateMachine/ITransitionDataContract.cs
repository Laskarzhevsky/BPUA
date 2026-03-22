using System.Collections.Generic;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines transition data contract functionality.
    /// </summary>
    public interface ITransitionDataContract
    {
        #region Properties
        /// <summary>
        /// Gets tables
        /// </summary>
        IReadOnlyList<ITransitionDataTableContract> Tables
        {
            get;
        }
        #endregion
    }
}
