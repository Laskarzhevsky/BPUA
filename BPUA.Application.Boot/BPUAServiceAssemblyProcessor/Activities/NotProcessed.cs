using System.Reflection;

using BPUA.Application.Contracts;
using BPUA.Core;

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
        /// <param name="serviceRegistry">Service registry</param>
        bool NotProcessed(Assembly loadedAssembly, IServiceRegistry serviceRegistry)
        {
            return serviceRegistry.TryMarkAssemblyFacet(loadedAssembly.FullName!, AssemblyFacet.Services);
        }
        #endregion
    }
}
