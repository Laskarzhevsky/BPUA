namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides BPUA platform bootstrapper functionality
    /// </summary>
    public partial class BpuaPlatformBootstrapper
    {
        #region Methods
        /// <summary>
        /// Initializes component
        /// </summary>
        /// <param name="pathToFolderWithExecutableFile">Path to folder with excutable file</param>
        void InitializeComponent(string pathToFolderWithExecutableFile)
        {
            PathToFolderWithExecutableFile = pathToFolderWithExecutableFile;
        }
        #endregion
    }
}
