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
        /// Determines whether loaded assembly has ProvideBPUAProcessors attribute
        /// </summary>
        /// <returns>Flag indicating whether loaded assembly has ProvideBPUAProcessors attribute</returns>
        bool HasProvideBPUAProcessorsAttribute()
        {
            bool hasProvideBPUAProcessorsAttribute = LoadedAssembly!.IsDefined(typeof(ProvideBPUAProcessorsAttribute), inherit: false);
            return hasProvideBPUAProcessorsAttribute;
        }
        #endregion
    }
}
