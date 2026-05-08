using System.Reflection;

using BPUA.Application.Contracts;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Processes assembly decorated by RegisterAsBPUATransitionAssemblyAttribute.
    /// Marks the assembly as processed by the transition-assembly registration pipeline.
    /// </summary>
    public partial class BpuaTransitionAssemblyProcessor : IBpuaAssemblyProcessor
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
                TransitionsRegistrar.RegisterTransitionsFromAssembly(loadedAssembly, serviceRegistry);
            }
        }
        #endregion
    }
}
