using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;

using Microsoft.Extensions.Configuration;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides BPUA platform bootstrapper functionality
    /// </summary>
    public sealed partial class BPUAPlatformBootstrapper
    {
        #region Methods
        void CalculatePathToFolderWithDynamicAssemblies()
        {
            PathToFolderWithDynamicAssemblies = AssemblyLoadingProcessConfigurator.CalculatePathToFolderWithDynamicAssemblies(ApplicationConfiguration, PathToFolderWithExecutableFile, IsDevelopmentEnvironment);
        }

        /// <summary>
        /// Initializes application
        /// </summary>
        void InitializeApplication()
        {
            BPUAApplication.GetInstance().Initialize(ApplicationConfiguration, PathToFolderWithDynamicAssemblies);
        }

        /// <summary>
        /// Initializes component
        /// </summary>
        /// <param name="pathToFolderWithExecutableFile">Path to folder with excutable file</param>
        /// <param name="isDevelopmentEnvironment">Flag indicating whether application runs in development environment</param>
        void InitializeComponent(string pathToFolderWithExecutableFile, bool isDevelopmentEnvironment)
        {
            PathToFolderWithExecutableFile = pathToFolderWithExecutableFile;
            IsDevelopmentEnvironment = isDevelopmentEnvironment;
        }

        /// <summary>
        /// Initializes use case activator
        /// </summary>
        void InitializeUseCaseActivator()
        {
            UseCaseActivator useCaseActivator = new UseCaseActivator();
            useCaseActivator.ListOfAssemblyProcessors = ListOfAssemblyProcessors;
            useCaseActivator.ListOfLoadedAssemblies = ListOfLoadedAssemblies;
            useCaseActivator.PathToFolderWithDynamicAssemblies = PathToFolderWithDynamicAssemblies;

            IServiceRegistry serviceRegistry = BPUAApplication.GetInstance().ServiceRegistry;
            serviceRegistry.RegisterObject(typeof(IUseCaseActivator).Name, useCaseActivator);
        }

        /// <summary>
        /// Loads application configuration
        /// </summary>
        void LoadApplicationConfiguration()
        {
            ApplicationConfiguration = AssemblyLoadingProcessConfigurator.LoadApplicationConfiguration();
        }

        /// <summary>
        /// Loads dynamic assemblies
        /// </summary>
        void LoadDynamicAssemblies()
        {
            IServiceRegistry serviceRegistry = BPUAApplication.GetInstance().ServiceRegistry;
            DynamicAssembliesLoader smartAssemblyLoader = new DynamicAssembliesLoader();
            smartAssemblyLoader.LoadDynamicAssemblies(PathToFolderWithDynamicAssemblies, serviceRegistry, ListOfLoadedAssemblies, ListOfAssemblyProcessors);
        }

        /// <summary>
        /// Loads static assemblies
        /// </summary>
        void LoadStaticAssemblies()
        {
            ListOfLoadedAssemblies.Add(typeof(BPUA.Application.BusinessLogic.AssemblyReference).Assembly);
            ListOfLoadedAssemblies.Add(typeof(BPUA.Application.DataAccessLogic.AssemblyReference).Assembly);
            ListOfLoadedAssemblies.Add(typeof(BPUA.Application.DataProcessingLogic.AssemblyReference).Assembly);
            ListOfLoadedAssemblies.Add(typeof(BPUA.Application.Orchestration.AssemblyReference).Assembly);
//            ListOfLoadedAssemblies.Add(typeof(BPUA.SqlServer.EventHandlers.AssemblyReference).Assembly);
        }

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
        /// Gets or sets flag indicating whether application runs in development environment
        /// </summary>
        bool IsDevelopmentEnvironment
        {
            get; set;
        }

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
