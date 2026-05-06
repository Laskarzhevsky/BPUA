using System;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides BPUA platform bootstrapper functionality
    /// </summary>
    public partial class BpuaPlatformBootstrapper
    {
        #region Methods
        /// <summary>
        /// Validates the executable folder path argument.
        /// </summary>
        /// <param name="pathToFolderWithExecutableFile">Path to folder with executable file.</param>
        void ValidatePathToFolderWithExecutableFile(string pathToFolderWithExecutableFile)
        {
            if (pathToFolderWithExecutableFile == null)
            {
                throw new ArgumentNullException(nameof(pathToFolderWithExecutableFile));
            }

            if (string.IsNullOrWhiteSpace(pathToFolderWithExecutableFile))
            {
                throw new ArgumentException("Path to folder with executable file cannot be empty or whitespace.", nameof(pathToFolderWithExecutableFile));
            }
        }
        #endregion
    }
}
