using System.Reflection;

using BPUA.Application.Contracts;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Processes assembly decorated by RegisterAsBPUARouteHandlerAssemblyAttribute.
    /// </summary>
    public partial class BpuaRouteHandlerAssemblyProcessor : IBpuaAssemblyProcessor
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
                RequestRoutesRegistrar.RegisterTransitionsFromAssembly(loadedAssembly, serviceRegistry);
            }
        }
        #endregion
    }
}
