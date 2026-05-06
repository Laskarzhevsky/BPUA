namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides BPUA platform bootstrapper functionality
    /// </summary>
    public partial class BpuaPlatformBootstrapper
    {
        #region Methods
        void CalculatePathToFolderWithDynamicAssemblies()
        {
            PathToFolderWithDynamicAssemblies = AssemblyLoadingProcessConfigurator.CalculatePathToFolderWithDynamicAssemblies(ApplicationConfiguration, PathToFolderWithExecutableFile);
        }
        #endregion
    }
}
