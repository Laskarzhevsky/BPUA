using System;

using BPUA.Application.Contracts;


namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides BPUA platform bootstrapper functionality
    /// </summary>
    public partial class BpuaPlatformBootstrapper
    {
        #region Methods
        /// <summary>
        /// Throws when the platform has already been bootstrapped.
        /// </summary>
        void ThrowIfAlreadyBootstrapped()
        {
            if (ManagementSystem.IsInitialized)
            {
                throw new InvalidOperationException("BPUA platform has already been bootstrapped.");
            }
        }
        #endregion
    }
}
