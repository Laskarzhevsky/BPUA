using System;
using System.Threading;
using System.Threading.Tasks;
using BPUA.Core;
using Microsoft.Extensions.Configuration;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Publishes the runtime application through its contract without an orchestration reference.
    /// The bootstrapper assigns the configured implementation before initializing services.
    /// </summary>
    public abstract class BpuaApplication : IBpuaApplication, IBpuaHostServices
    {
        #region Static Members
        /// <summary>
        /// Holds reference to the instance of the BPUA application
        /// </summary>
        private static BpuaApplication? _application;

        /// <summary>
        /// Gets whether an application has been assigned
        /// </summary>
        public static bool IsInitialized
        {
            get { return Volatile.Read(ref _application) != null; }
        }

        /// <summary>
        /// Gets BPUA service
        /// </summary>
        /// <param name="bpuaServicekey">BPUA service key</param>
        /// <returns>BPUA service</returns>
        public static async Task<IBpuaService?> GetBpuaServiceAsync(string? bpuaServicekey)
        {
            if (string.IsNullOrEmpty(bpuaServicekey))
            {
                return null;
            }

            bpuaServicekey = bpuaServicekey.Trim('/');
            IBpuIdentifier bpuIdentifier = new BpuIdentifier(bpuaServicekey);
            UseCaseActivationResult useCaseActivationResult = await Instance.ActivateUseCaseAsync(bpuIdentifier);
            if (useCaseActivationResult.Succeeded)
            {
                return Instance.GetRequestHandler(bpuaServicekey);
            }

            return null;
        }

        /// <summary>
        /// Gets the application assigned during platform startup.
        /// </summary>
        public static BpuaApplication Instance
        {
            get
            {
                BpuaApplication? application = Volatile.Read(ref _application);
                if (application == null)
                {
                    throw new InvalidOperationException("BPUA application has not been initialized. Boot the BPUA platform before accessing application services.");
                }
                return application;
            }
        }

        /// <summary>
        /// Assigns the runtime application once. Reassigning the same instance is harmless.
        /// </summary>
        public static void Initialize(BpuaApplication application)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }
            BpuaApplication? existing = Interlocked.CompareExchange(ref _application, application, null);
            if (existing != null && !object.ReferenceEquals(existing, application))
            {
                throw new InvalidOperationException("BPUA application has already been initialized with another application.");
            }
        }
        #endregion

        #region Abstract Members
        /// <summary>
        /// Activates a use case
        /// </summary>
        /// <param name="bpuIdentifier">BPUA identifier</param>
        /// <returns></returns>
        public abstract Task<UseCaseActivationResult> ActivateUseCaseAsync(IBpuIdentifier bpuIdentifier);

        /// <summary>
        /// Executes a transition.
        /// </summary>
        /// <param name="bpuIdentifier">BPUA identifier</param>
        public abstract Task ExecuteTransition(IBpuIdentifier bpuIdentifier);

        /// <summary>
        /// Gets a registered request handler.
        /// </summary>
        /// <param name="requesthandlerKey">The key for the request handler</param>
        /// <returns>The request handler, or null if not found</returns>
        public abstract IRequestHandler? GetRequestHandler(string requesthandlerKey);

        /// <summary>
        /// Gets an application configuration value.
        /// </summary>
        /// <param name="key">The key for the configuration value</param>
        /// <returns>The configuration value, or null if not found</returns>
        public abstract T? GetValueFromApplicationConfiguration<T>(string key);

        /// <summary>
        /// Configures the runtime before publication.
        /// </summary>
        /// <param name="applicationConfiguration">The application configuration</param>
        /// <param name="pathToFolderWithDynamicAssemblies">The path to the folder with dynamic assemblies</param>
        public abstract void Initialize(IConfiguration applicationConfiguration, string pathToFolderWithDynamicAssemblies);

        /// <summary>
        /// Initializes hosted layers.
        /// </summary>
        public abstract Task InitializeHostedApplicationLayers();

        /// <summary>
        /// Checks whether a use case is activated.
        /// </summary>
        /// <param name="useCaseKey">The key for the use case</param>
        /// <returns>true if the use case is activated, false otherwise</returns>
        public abstract bool IsUseCaseActivated(string useCaseKey);

        /// <summary>
        /// Dispatches a service request.
        /// </summary>
        /// <param name="eventSource">The source of the event</param>
        /// <param name="args">The service request arguments</param>
        public abstract Task RequestHandler_RequestServiceEvent(object? eventSource, ServiceRequestEventArgs args);

        /// <summary>
        /// Gets a connection string for an application layer.
        /// </summary>
        /// <param name="bpuIdentifier">The BPU identifier</param>
        /// <returns>The connection string</returns>
        public abstract string GetConnectionString(IBpuIdentifier bpuIdentifier);

        /// <summary>
        /// Subscribes to a handler's service requests.
        /// </summary>
        /// <param name="requestHandler">The request handler</param>
        public abstract void SignInToRequestHandlerRequestServiceEvent(IRequestHandler requestHandler);

        /// <summary>
        /// Unsubscribes from a handler's service requests.</summary>
        /// <param name="requestHandler">The request handler</param>
        public abstract void SignOutFromRequestHandlerRequestServiceEvent(IRequestHandler requestHandler);

        /// <summary>Gets application configuration.</summary>
        public abstract IConfiguration ApplicationConfiguration
        {
            get; protected set; 
        }

        /// <summary>
        /// Gets the dynamic assembly folder.
        /// </summary>
        public abstract string PathToFolderWithDynamicAssemblies 
        {
            get; protected set; 
        }

        /// <summary>
        /// Gets the application service registry.
        /// </summary>
        public abstract IServiceRegistry ServiceRegistry 
        { 
            get; protected set; 
        }
        #endregion
    }
}
