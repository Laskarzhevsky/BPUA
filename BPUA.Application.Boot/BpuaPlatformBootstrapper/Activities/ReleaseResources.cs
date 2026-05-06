namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides BPUA platform bootstrapper functionality
    /// </summary>
    public partial class BpuaPlatformBootstrapper
    {
        #region Methods
        /// <summary>
        /// Releases resources
        /// </summary>
        void ReleaseResources()
        {
            ApplicationConfiguration = default!;
            ListOfAssemblyProcessors = default!;
            ListOfLoadedAssemblies = default!;
        }
        #endregion
    }
}
