namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides BPUA platform bootstrapper functionality
    /// </summary>
    public partial class BpuaPlatformBootstrapper
    {
        #region Methods
        /// <summary>
        /// Loads application configuration
        /// </summary>
        void LoadApplicationConfiguration()
        {
            ApplicationConfiguration = AssemblyLoadingProcessConfigurator.LoadApplicationConfiguration(PathToFolderWithExecutableFile);
        }
        #endregion
    }
}
