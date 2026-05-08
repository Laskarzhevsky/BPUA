using System.Reflection;

using BPUA.Application.Contracts;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Processes assembly decorated by RegisterAsBPUATransitionAssemblyAttribute
    /// Marks the assembly as processed by the service-assembly registration pipeline.
    /// </summary>
    public partial class BpuaTransitionAssemblyProcessor
    {
        #region Methods
        /// <summary>
        /// Gets flag indicating whether processor can process loaded assembly
        /// </summary>
        /// <param name="loadedAssembly">Loaded assembly</param>
        bool CanProcess(Assembly loadedAssembly)
        {
            return loadedAssembly.IsDefined(typeof(RegisterAsBPUATransitionAssemblyAttribute), inherit: false);
        }
        #endregion
    }
}
