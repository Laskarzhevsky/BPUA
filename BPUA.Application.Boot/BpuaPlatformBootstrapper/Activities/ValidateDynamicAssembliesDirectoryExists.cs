using System.IO;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides BPUA platform bootstrapper functionality
    /// </summary>
    public partial class BpuaPlatformBootstrapper
    {
        #region Methods
        /// <summary>
        /// Validates that the dynamic assemblies directory exists.
        /// </summary>
        void ValidateDynamicAssembliesDirectoryExists()
        {
            if (!Directory.Exists(PathToFolderWithDynamicAssemblies))
            {
                throw new DirectoryNotFoundException("The plugin folder does not exist: " + PathToFolderWithDynamicAssemblies);
            }
        }
        #endregion
    }
}
