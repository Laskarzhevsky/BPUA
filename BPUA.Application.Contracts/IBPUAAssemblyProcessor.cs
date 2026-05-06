using System.Reflection;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Post-load processor that inspects an assembly and performs registrations.
    /// </summary>
    public interface IBpuaAssemblyProcessor
    {
        #region Methods
        /// <summary>
        /// Processes loaded assembly
        /// </summary>
        /// <param name="loadedAssembly">Loaded assembly</param>
        /// <param name="serviceRegistry">Service registry</param>
        void Process(Assembly loadedAssembly, IServiceRegistry serviceRegistry);
        #endregion
    }
}
