using System.Reflection;

using BPUA.Application.Contracts;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Processes assembly decorated by RegisterAsBpuaServiceAssemblyAttribute
    /// Marks the assembly as processed by the service-assembly registration pipeline.
    /// This pipeline currently registers both BPUA services and transition definitions.
    /// </summary>
    public partial class BpuaServiceAssemblyProcessor
    {
        #region Methods
        /// <summary>
        /// Gets flag indicating whether processor can process loaded assembly
        /// </summary>
        /// <param name="loadedAssembly">Loaded assembly</param>
        bool CanProcess(Assembly loadedAssembly)
        {
            return loadedAssembly.IsDefined(typeof(RegisterAsBpuaServiceAssemblyAttribute), inherit: false);
        }
        #endregion
    }
}
