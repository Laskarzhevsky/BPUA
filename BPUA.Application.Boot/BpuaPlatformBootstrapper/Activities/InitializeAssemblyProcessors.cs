namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides BPUA platform bootstrapper functionality
    /// </summary>
    public partial class BpuaPlatformBootstrapper
    {
        #region Methods
        /// <summary>
        /// Initializes the list of assembly processors required during platform boot.
        /// The service assembly processor is needed so static platform assemblies marked
        /// with RegisterAsBpuaServiceAssembly can register their built-in services.
        /// </summary>
        void InitializeAssemblyProcessors()
        {
            bool exists = false;
            for (int i = 0; i < ListOfAssemblyProcessors.Count; i++)
            {
                if (ListOfAssemblyProcessors[i] is BpuaServiceAssemblyProcessor)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                ListOfAssemblyProcessors.Add(new BpuaServiceAssemblyProcessor());
            }
        }
        #endregion
    }
}
