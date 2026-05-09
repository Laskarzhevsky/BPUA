using System.Reflection;

using BPUA.Application.Contracts;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Processes assembly decorated by RegisterAsBPUARouteHandlerAssemblyAttribute
    /// Marks the assembly as processed by the transition-assembly registration pipeline.
    /// </summary>
    public partial class BpuaRouteHandlerAssemblyProcessor
    {
        #region Methods
        /// <summary>
        /// Gets flag indicating whether processor can process loaded assembly
        /// </summary>
        /// <param name="loadedAssembly">Loaded assembly</param>
        bool CanProcess(Assembly loadedAssembly)
        {
            return loadedAssembly.IsDefined(typeof(RegisterAsBPUARouteHandlerAssemblyAttribute), inherit: false);
        }
        #endregion
    }
}
