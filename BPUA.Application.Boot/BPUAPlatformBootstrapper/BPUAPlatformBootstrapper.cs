using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using BPUA.Application.Contracts;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides BPUA platform bootstrapper functionality
    /// </summary>
    public sealed partial class BPUAPlatformBootstrapper
    {
        #region Public Methods
        /// <summary>
        /// Boots BPUA platform
        /// </summary>
        /// <param name="pathToFolderWithExecutableFile">Path to folder with excutable file</param>
        public async Task BootBPUAPlatform(string pathToFolderWithExecutableFile)
        {
            ThrowIfAlreadyBootstrapped();
            ValidatePathToFolderWithExecutableFile(pathToFolderWithExecutableFile);
            InitializeComponent(pathToFolderWithExecutableFile);

            try
            {
                LoadApplicationConfiguration();
                CalculatePathToFolderWithDynamicAssemblies();
                ValidateDynamicAssembliesDirectoryExists();
                LoadStaticAssemblies();
                InitializeApplication();
                BuildDynamicAssembliesPathIndex();
                InitializeAssemblyProcessors();
                ProcessStaticAssemblies();
                InitializeUseCaseActivator();
                await InitializeHostedApplicationLayers();
            }
            finally
            {
                ReleaseResources();
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets list of assembly processors
        /// </summary>
        public List<IBPUAAssemblyProcessor> ListOfAssemblyProcessors
        {
            get; set;
        } = new List<IBPUAAssemblyProcessor>();

        /// <summary>
        /// Gets or sets list of loaded assemblies
        /// </summary>
        public List<Assembly> ListOfLoadedAssemblies
        {
            get; set;
        } = new List<Assembly>();
        #endregion
    }
}
