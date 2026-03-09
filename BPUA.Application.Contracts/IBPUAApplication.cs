using System;
using System.Threading.Tasks;

using BPUA.Core;

using Microsoft.Extensions.Configuration;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines BPUA application functionality
    /// </summary>
    public interface IBPUAApplication
    {
        #region Methods
        /// <summary>
        /// Activates use case
        /// </summary>
        /// <param name="bpuaIdentifier">BPUA identifier</param>
        /// <returns>Use case activation result</returns>
        Task<UseCaseActivationResult> ActivateUseCaseAsync(IBPUAIdentifier bpuaIdentifier);

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
        /// Gets flag indicating whether use case activated
        /// </summary>
        /// <param name="useCaseKey">Use case key</param>
        /// <returns>Flag indicating whether use case activated</returns>
        bool IsUseCaseActivated(string useCaseKey);
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets application configuration
        /// </summary>
        IConfiguration ApplicationConfiguration
        {
            get; set;
        }

        /// <summary>
        /// Gets service registry 
        /// </summary>
        IServiceRegistry ServiceRegistry
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
        #endregion

        #region Event handlers
        /// <summary>
        /// Handles RequestHandler.RequestService event
        /// </summary>
        /// <param name="eventSource">Event source</param>
        /// <param name="args">Event arguments</param>
        Task RequestHandler_RequestServiceEvent(object? eventSource, EventArgs args);
        #endregion
    }
}
