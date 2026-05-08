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
        /// Applies all configured assembly processors to the currently loaded assembly.
        /// </summary>
        void ProcessLoadedAssembly()
        {
            if (LoadedAssembly == null)
            {
                return;
            }

            for (int i = 0; i < ListOfAssemblyProcessors.Count; i++)
            {
                IBpuaAssemblyProcessor bpuaAssemblyProcessor = ListOfAssemblyProcessors[i];
                bpuaAssemblyProcessor.Process(LoadedAssembly, ServiceRegistry);
            }
        }
        #endregion
    }
}
