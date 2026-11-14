using BPUA.Application.Contracts;
using BPUA.Core;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Represents the runtime application coordinator that listens to service requests
    /// and dispatches them to the appropriate event bus handler.
    /// </summary>
    public class BpuaApplication : BPUA.Application.Contracts.BpuaApplication
    {
        #region Data Fields
        /// <summary>
        /// Holds state machines for activated use cases. Key is BPU identifier key.
        /// </summary>
        readonly Dictionary<string, StateMachine> _stateMachines = new Dictionary<string, StateMachine>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates the runtime application. The bootstrapper publishes it through the Contracts BpuaApplication.
        /// </summary>
        public BpuaApplication()
        {
            ServiceRegistry = new ServiceRegistry();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Activates use case
        /// IBpuaApplication interface implementation
        /// </summary>
        /// <param name="bpuIdentifier">BPU identifier</param>
        /// <returns>Use case activation result</returns>
        public override async Task<UseCaseActivationResult> ActivateUseCaseAsync(IBpuIdentifier bpuIdentifier)
        {
            IUseCaseActivator useCaseActivator = ServiceRegistry.GetObject<IUseCaseActivator>(typeof(IUseCaseActivator).Name);
            UseCaseActivationResult useCaseActivationResult = await useCaseActivator.ActivateAsync(bpuIdentifier, ServiceRegistry);
            return useCaseActivationResult;
        }

        /// <summary>
        /// Executes transition
        /// IBpuaApplication interface implementation
        /// </summary>
        /// <param name="bpuIdentifier">BPU identifier</param>
        public override async Task ExecuteTransition(IBpuIdentifier bpuIdentifier)
        {
            string stateMachineKey = KeyCompiler.CompileHostedApplicationLayerKey(bpuIdentifier.DomainName, bpuIdentifier.UseCaseName, bpuIdentifier.ApplicationLayerName);
            StateMachine? stateMachine = null;
            if (_stateMachines.ContainsKey(stateMachineKey))
            {
                stateMachine = _stateMachines[stateMachineKey];
            }
            else
            {
                stateMachine = new StateMachine();
                _stateMachines[stateMachineKey] = stateMachine;
            }

            await stateMachine.ExecuteTransition(this, bpuIdentifier);
        }

        /// <summary>
        /// Gets the connection string for the specified BPU identifier.
        /// IBpuaHostServices interface implementation
        /// </summary>
        /// <param name="bpuIdentifier">BPU identifier</param>
        /// <returns>Connection string</returns>
        public override string GetConnectionString(IBpuIdentifier bpuIdentifier)
        {
            string hostedApplicationLayerKey = KeyCompiler.CompileHostedApplicationLayerKey(bpuIdentifier.DomainName, bpuIdentifier.UseCaseName, bpuIdentifier.ApplicationLayerName);
            string? connectionString = ApplicationConfiguration.GetConnectionString(hostedApplicationLayerKey);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ApplicationException("Connection string not found for application layer: " + hostedApplicationLayerKey);
            }

            return connectionString;
        }

        /// <summary>
        /// Gets instance of BPUA application
        /// </summary>
        /// <returns>BPUA application</returns>
        public static IBpuaApplication GetInstance()
        {
            return BPUA.Application.Contracts.BpuaApplication.Instance;
        }

        /// <summary>
        /// Gets request handler
        /// IBpuaApplication interface implementation
        /// </summary>
        /// <param name="requesthandlerKey">Request handler key</param>
        /// <returns>Request handler</returns>
        public override IRequestHandler? GetRequestHandler(string requesthandlerKey)
        {
            IRequestHandler? requestHandler = (IRequestHandler?)ServiceRegistry.GetBpuaService(requesthandlerKey);
            return requestHandler;
        }

        /// <summary>
        /// Gets value from application configuration
        /// IBpuaApplication interface implementation
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="key">Value key</param>
        /// <returns>Value from application configuration</returns>
        public override T? GetValueFromApplicationConfiguration<T>(string key) where T : default
        {
            T? value = ApplicationConfiguration.GetValue<T>(key);
            return value;
        }

        /// <summary>
        /// Initializes BPUA application
        /// </summary>
        /// <param name="applicationConfiguration">Application configuration</param>
        /// <param name="pathToFolderWithDynamicAssemblies">Path to folder with dynamic assemblies</param>
        public override void Initialize(IConfiguration applicationConfiguration, string pathToFolderWithDynamicAssemblies)
        {
            ApplicationConfiguration = applicationConfiguration;
            PathToFolderWithDynamicAssemblies = pathToFolderWithDynamicAssemblies;
        }


        /// <summary>
        /// Initializes hosted application layers
        /// IBpuaApplication interface implementation
        /// </summary>
        public override async Task InitializeHostedApplicationLayers()
        {
            await HostedApplicationLayersInitializer.Initialize(this);
        }

        /// <summary>
        /// Gets flag indicating whether use case activated
        /// IBpuaApplication interface implementation
        /// </summary>
        /// <param name="useCaseKey">Use case key</param>
        /// <returns>Flag indicating whether use case activated</returns>
        public override bool IsUseCaseActivated(string useCaseKey)
        {
            // TODO
            return true;
        }

        /// <summary>
        /// Signs in to request handler request service event
        /// </summary>
        /// <param name="requestHandler">Request handler</param>
        public override void SignInToRequestHandlerRequestServiceEvent(IRequestHandler requestHandler)
        {
            requestHandler.ServiceRequestEvent += RequestHandler_RequestServiceEvent;
        }

        /// <summary>
        /// Signs out from request handler request service event
        /// </summary>
        /// <param name="requestHandler">Request handler</param>
        public override void SignOutFromRequestHandlerRequestServiceEvent(IRequestHandler requestHandler)
        {
            requestHandler.ServiceRequestEvent -= RequestHandler_RequestServiceEvent;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets application configuration
        /// IBpuaApplication interface implementation
        /// </summary>
        public override IConfiguration ApplicationConfiguration
        {
            get; protected set;
        } = default!;

        /// <summary>
        /// Gets path to folder with dynamic assemblies
        /// IBpuaApplication interface implementation
        /// </summary>
        public override string PathToFolderWithDynamicAssemblies
        {
            get; protected set;
        } = default!;

        /// <summary>
        /// Gets service registry 
        /// IBpuaApplication interface implementation
        /// </summary>
        public override IServiceRegistry ServiceRegistry
        {
            get; protected set;
        }
        #endregion

        #region Event handlers
        /// <summary>
        /// Handles RequestHandler.RequestService event
        /// IBpuaApplication interface implementaion
        /// </summary>
        /// <param name="eventSource">Event source</param>
        /// <param name="args">Event arguments</param>
        public override async Task RequestHandler_RequestServiceEvent(object? eventSource, ServiceRequestEventArgs args)
        {
            EventArgs eventArguments = args.EventArguments;
            IBpuaService? bppService = ServiceRegistry.GetBpuaService(eventArguments);
            if (bppService != null)
            {
                await using (bppService as IAsyncDisposable)
                {
                    await bppService.InitializeComponent(this);

                    // Seding BPUA application as an event source to BPUA serviced instance
                    await bppService.HandleAsync(this, args);
                }
            }
        }
        #endregion
    }
}
