using System;

using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;

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
            IBpuaApplication application = BpuaApplication.GetInstance();
            if (!string.IsNullOrWhiteSpace(application.PathToFolderWithDynamicAssemblies))
            {
                throw new InvalidOperationException("BPUA platform has already been bootstrapped.");
            }
        }
        #endregion
    }
}
