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
    public class BPUAApplication : IBPUAApplication
    {
        #region Data Fields
        /// <summary>
        /// Holds reference to BPUA application
        /// </summary>
        static BPUAApplication? _bppApplication = null;

        /// <summary>
        /// Holds state machines for activated use cases. Key is BPUA identifier key.
        /// </summary>
        readonly Dictionary<string, StateMachine> _stateMachines = new Dictionary<string, StateMachine>();
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        BPUAApplication()
        {
            ServiceRegistry = new ServiceRegistry();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Activates use case
        /// IBPUAApplication interface implementation
        /// </summary>
        /// <param name="bpuaIdentifier">BPUA identifier</param>
        /// <returns>Use case activation result</returns>
        public async Task<UseCaseActivationResult> ActivateUseCaseAsync(IBPUAIdentifier bpuaIdentifier)
        {
            IUseCaseActivator useCaseActivator = ServiceRegistry.GetObject<IUseCaseActivator>(typeof(IUseCaseActivator).Name);
            UseCaseActivationResult useCaseActivationResult = await useCaseActivator.ActivateAsync(bpuaIdentifier, ServiceRegistry);
            return useCaseActivationResult;
        }

        /// <summary>
        /// Executes transition
        /// IBPUAApplication interface implementation
        /// </summary>
        /// <param name="bpuaIdentifier">BPUA identifier</param>
        public async Task ExecuteTransition(IBPUAIdentifier bpuaIdentifier)
        {
            string stateMachineKey = KeyCompiler.CompileHostedApplicationLayerKey(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName);
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

            await stateMachine.ExecuteTransition(this, bpuaIdentifier);
        }

        /// <summary>
        /// Gets instance of BPUA application
        /// </summary>
        /// <returns>BPUA application</returns>
        public static IBPUAApplication GetInstance()
        {
            if (_bppApplication == null)
            {
                _bppApplication = new BPUAApplication();
            }

            return _bppApplication;
        }

        /// <summary>
        /// Gets request handler
        /// IBPUAApplication interface implementation
        /// </summary>
        /// <param name="requesthandlerKey">Request handler key</param>
        /// <returns>Request handler</returns>
        public IRequestHandler? GetRequestHandler(string requesthandlerKey)
        {
            IRequestHandler? requestHandler = (IRequestHandler?)ServiceRegistry.GetBPUAService(requesthandlerKey);
            return requestHandler;
        }

        /// <summary>
        /// Gets value from application configuration
        /// IBPUAApplication interface implementation
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="key">Value key</param>
        /// <returns>Value from application configuration</returns>
        public T? GetValueFromApplicationConfiguration<T>(string key)
        {
            T? value = ApplicationConfiguration.GetValue<T>(key);
            return value;
        }

        /// <summary>
        /// Initializes BPUA application
        /// </summary>
        /// <param name="applicationConfiguration">Application configuration</param>
        /// <param name="pathToFolderWithDynamicAssemblies">Path to folder with dynamic assemblies</param>
        public void Initialize(IConfiguration applicationConfiguration, string pathToFolderWithDynamicAssemblies)
        {
            ApplicationConfiguration = applicationConfiguration;
            PathToFolderWithDynamicAssemblies = pathToFolderWithDynamicAssemblies;
        }


        /// <summary>
        /// Initializes hosted application layers
        /// IBPUAApplication interface implementation
        /// </summary>
        public async Task InitializeHostedApplicationLayers()
        {
            await HostedApplicationLayersInitializer.Initialize(this);
        }

        /// <summary>
        /// Gets flag indicating whether use case activated
        /// IBPUAApplication interface implementation
        /// </summary>
        /// <param name="useCaseKey">Use case key</param>
        /// <returns>Flag indicating whether use case activated</returns>
        public bool IsUseCaseActivated(string useCaseKey)
        {
            // TODO
            return true;
        }

        /// <summary>
        /// Signs in to request handler request service event
        /// </summary>
        /// <param name="requestHandler">Request handler</param>
        public void SignInToRequestHandlerRequestServiceEvent(IRequestHandler requestHandler)
        {
            requestHandler.ServiceRequestEvent += RequestHandler_RequestServiceEvent;
        }

        /// <summary>
        /// Signs out from request handler request service event
        /// </summary>
        /// <param name="requestHandler">Request handler</param>
        public void SignOutFromRequestHandlerRequestServiceEvent(IRequestHandler requestHandler)
        {
            requestHandler.ServiceRequestEvent -= RequestHandler_RequestServiceEvent;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets application configuration
        /// IBPUAApplication interface implementation
        /// </summary>
        public IConfiguration ApplicationConfiguration
        {
            get; private set;
        } = default!;

        /// <summary>
        /// Gets path to folder with dynamic assemblies
        /// IBPUAApplication interface implementation
        /// </summary>
        public string PathToFolderWithDynamicAssemblies
        {
            get; private set;
        } = default!;

        /// <summary>
        /// Gets service registry 
        /// IBPUAApplication interface implementation
        /// </summary>
        public IServiceRegistry ServiceRegistry
        {
            get; private set;
        }
        #endregion

        #region Event handlers
        /// <summary>
        /// Handles RequestHandler.RequestService event
        /// IBPUAApplication interface implementaion
        /// </summary>
        /// <param name="eventSource">Event source</param>
        /// <param name="args">Event arguments</param>
        public async Task RequestHandler_RequestServiceEvent(object? eventSource, ServiceRequestEventArgs args)
        {
            EventArgs eventArguments = args.EventArguments;
            IBPUAService? bppService = ServiceRegistry.GetBPUAService(eventArguments);
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
