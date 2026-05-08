using System.Reflection;

using BPUA.Application.Contracts;
using BPUA.Core;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Processes assembly decorated by RegisterAsBPUATransitionAssemblyAttribute
    /// Marks the assembly as processed by the transition-assembly registration pipeline.
    /// </summary>
    public partial class BpuaTransitionAssemblyProcessor
    {
        #region Methods
        /// <summary>
        /// Gets flag indicating whether processor can process loaded assembly
        /// </summary>
        /// <param name="loadedAssembly">Loaded assembly</param>
        /// <param name="serviceRegistry">Service registry</param>
        bool NotProcessed(Assembly loadedAssembly, IServiceRegistry serviceRegistry)
        {
            return serviceRegistry.TryMarkAssemblyFacet(loadedAssembly.FullName!, AssemblyFacet.Services);
        }
        #endregion
    }
}
