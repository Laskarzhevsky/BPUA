using BPUA.Application.Contracts;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides dynamic assemblies loader functionality
    /// </summary>
    public partial class DynamicAssembliesLoader
    {
        #region Methods
        /// <summary>
        /// Determines whether loaded assembly has LoadBPUAAssembly attribute
        /// </summary>
        /// <returns>Flag indicating whether loaded assembly has LoadBPUAAssembly attribute</returns>
        bool HasLoadBPUAAssemblyAttribute()
        {
            bool hasLoadBPUAAssemblyAttribute = LoadedAssembly!.IsDefined(typeof(LoadBPUAAssemblyAttribute), inherit: false);
            return hasLoadBPUAAssemblyAttribute;
        }
        #endregion
    }
}
