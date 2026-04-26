using System;

namespace BPUA.Application.NonFunctionalContracts
{
    /// <summary>
    /// Defines receiver controller functionality
    /// </summary>
    public interface IReceiverController : IDisposable
    {
        #region Properties
        /// <summary>
        /// Gets flag indicating whether application initialized
        /// </summary>
        bool ApplicationInitialized
        {
            get;
        }

        /// <summary>
        /// Gets host URL
        /// </summary>
        string? HostUrl
        {
            get;
        }
        #endregion
    }
}
