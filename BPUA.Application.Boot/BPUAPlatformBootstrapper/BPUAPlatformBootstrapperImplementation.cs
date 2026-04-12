using System;
using System.IO;

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
        /// <summary>
        /// Builds the dynamic assemblies path index
        /// </summary>
        void BuildDynamicAssembliesPathIndex()
        {
            IBPUAApplication application = BPUAApplication.GetInstance();
            DynamicAssemblyPathIndexBuilder.BuildAssemblyPathIndex(application.PathToFolderWithDynamicAssemblies, application.ServiceRegistry);
        }

        /// <summary>
        /// Throws when the platform has already been bootstrapped.
        /// </summary>
        void ThrowIfAlreadyBootstrapped()
        {
            IBPUAApplication application = BPUAApplication.GetInstance();
            if (!string.IsNullOrWhiteSpace(application.PathToFolderWithDynamicAssemblies))
            {
                throw new InvalidOperationException("BPUA platform has already been bootstrapped.");
            }
        }

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
        /// Loads dynamic assemblies.
        /// Dynamic plugin assemblies are no longer loaded during platform boot.
        /// They are loaded on demand by UseCaseActivator based on IBPUAIdentifier.
        /// </summary>
        void LoadDynamicAssemblies()
        {
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
        /// Initializes the list of assembly processors required during platform boot.
        /// The service assembly processor is needed so static platform assemblies marked
        /// with RegisterAsBPUAServiceAssembly can register their built-in services.
        /// </summary>
        void InitializeAssemblyProcessors()
        {
            bool exists = false;
            for (int i = 0; i < ListOfAssemblyProcessors.Count; i++)
            {
                if (ListOfAssemblyProcessors[i] is BPUAServiceAssemblyProcessor)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                ListOfAssemblyProcessors.Add(new BPUAServiceAssemblyProcessor());
            }
        }

        /// <summary>
        /// Processes the already loaded static platform assemblies through all configured
        /// assembly processors so that built-in handlers and routers are registered during boot.
        /// Dynamic plugin assemblies are still loaded only on demand later by UseCaseActivator.
        /// </summary>
        void ProcessStaticAssemblies()
        {
            IServiceRegistry serviceRegistry = BPUAApplication.GetInstance().ServiceRegistry;

            for (int i = 0; i < ListOfLoadedAssemblies.Count; i++)
            {
                System.Reflection.Assembly loadedAssembly = ListOfLoadedAssemblies[i];

                for (int j = 0; j < ListOfAssemblyProcessors.Count; j++)
                {
                    IBPUAAssemblyProcessor assemblyProcessor = ListOfAssemblyProcessors[j];
                    assemblyProcessor.Process(loadedAssembly, serviceRegistry);
                }
            }
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
