using BPUA.Core;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines BPUA application host services functionality
    /// </summary>
    public interface IBpuaHostServices
    {
        #region Methods
        /// <summary>
        /// Gets the connection string for the specified BPU identifier.
        /// </summary>
        /// <param name="bpuIdentifier">BPU identifier</param>
        /// <returns>Connection string</returns>
        string GetConnectionString(IBpuIdentifier bpuIdentifier);
        #endregion
    }
}
