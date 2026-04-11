using System;
using System.Threading.Tasks;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines BPUA application service functionality
    /// </summary>
    public interface IBPUAService
    {
        #region Methods
        /// <summary>
        /// Initializes component
        /// </summary>
        /// <param name="bppApplication">BPUA application</param>
        Task InitializeComponent(IBPUAApplication bppApplication);
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles event asynchronously
        /// </summary>
        /// <param name="sender">Event source</param>
        /// <param name="args">Event arguments</param>
        Task HandleAsync(object? sender, EventArgs args);
        #endregion
    }
}
