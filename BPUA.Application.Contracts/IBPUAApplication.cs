using System.Threading.Tasks;

using BPUA.Core;

using Microsoft.Extensions.Configuration;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines BPUA application functionality
    /// </summary>
    public interface IBpuaApplication
    {
        #region Methods
        /// <summary>
        /// Executes transition
        /// </summary>
        /// <param name="bpuIdentifier">BPU identifier</param>
        Task ExecuteTransition(IBpuIdentifier bpuIdentifier);

        /// <summary>
        /// Gets request handler
        /// </summary>
        /// <param name="requesthandlerKey">Request handler key</param>
        /// <returns>Request handler</returns>
        IRequestHandler? GetRequestHandler(string requesthandlerKey);

        /// <summary>
        /// Gets value from application configuration
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="key">Value key</param>
        /// <returns>Value from application configuration</returns>
        T? GetValueFromApplicationConfiguration<T>(string key);

        /// <summary>
        /// Initializes BPUA application
        /// </summary>
        /// <param name="applicationConfiguration">Application configuration</param>
        /// <param name="pathToFolderWithDynamicAssemblies">Path to folder with dynamic assemblies</param>
        void Initialize(IConfiguration applicationConfiguration, string pathToFolderWithDynamicAssemblies);

        /// <summary>
        /// Initializes hosted application layers
        /// </summary>
        Task InitializeHostedApplicationLayers();

        /// <summary>
        /// Gets flag indicating whether use case activated
        /// </summary>
        /// <param name="useCaseKey">Use case key</param>
        /// <returns>Flag indicating whether use case activated</returns>
        bool IsUseCaseActivated(string useCaseKey);
        #endregion

        #region Properties
        /// <summary>
        /// Gets application configuration
        /// </summary>
        IConfiguration ApplicationConfiguration
        {
            get;
        }

        /// <summary>
        /// Gets path to folder with dynamic assemblies
        /// </summary>
        string PathToFolderWithDynamicAssemblies
        {
            get;
        }

        /// <summary>
        /// Gets service registry 
        /// </summary>
        IServiceRegistry ServiceRegistry
        {
            get;
        }
        #endregion

        #region Event handlers
        /// <summary>
        /// Handles RequestHandler.RequestService event
        /// </summary>
        /// <param name="eventSource">Event source</param>
        /// <param name="args">Event arguments</param>
        Task RequestHandler_RequestServiceEvent(object? eventSource, ServiceRequestEventArgs args);
        #endregion
    }
}
