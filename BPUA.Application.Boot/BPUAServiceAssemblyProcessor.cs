using System.Reflection;

using BPUA.Application.Contracts;
using BPUA.Core;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Processes assembly decorated by RegisterAsBPUAServiceAssemblyAttribute
    /// Marks the assembly as processed by the service-assembly registration pipeline.
    /// This pipeline currently registers both BPUA services and transition definitions.
    /// </summary>
    public sealed class BPUAServiceAssemblyProcessor : IBPUAAssemblyProcessor
    {
        #region Public Methods
        /// <summary>
        /// Processes loaded assembly
        /// IBPUAAssemblyProcessor interface implementation
        /// </summary>
        /// <param name="loadedAssembly">Loaded assembly</param>
        /// <param name="serviceRegistry">Service registry</param>
        public void Process(Assembly loadedAssembly, IServiceRegistry serviceRegistry)
        {
            if (CanProcess(loadedAssembly) && NotProcessed(loadedAssembly, serviceRegistry))
            {
                BPUAServicesRegistrar.RegisterServicesFromAssembly(loadedAssembly, serviceRegistry);
                TransitionsRegistrar.RegisterTransitionsFromAssembly(loadedAssembly, serviceRegistry);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets flag indicating whether processor can process loaded assembly
        /// </summary>
        /// <param name="loadedAssembly">Loaded assembly</param>
        bool CanProcess(Assembly loadedAssembly)
        {
            return loadedAssembly.IsDefined(typeof(RegisterAsBPUAServiceAssemblyAttribute), inherit: false);
        }

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
