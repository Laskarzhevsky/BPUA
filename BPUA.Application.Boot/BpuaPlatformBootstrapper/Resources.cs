using Microsoft.Extensions.Configuration;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides BPUA platform bootstrapper functionality
    /// </summary>
    public partial class BpuaPlatformBootstrapper
    {
        #region Properties
        /// <summary>
        /// Gets or sets application configuration
        /// </summary>
        IConfiguration ApplicationConfiguration
        {
            get;
            set;
        } = default!;

        /// <summary>
        /// Gets or sets path to folder with excutable file
        /// </summary>
        string PathToFolderWithExecutableFile
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets path to folder with dynamic assemblies
        /// </summary>
        string PathToFolderWithDynamicAssemblies
        {
            get; set;
        } = default!;
        #endregion
    }
}
