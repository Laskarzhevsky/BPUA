using System;
using System.IO;

using BPUA.Application.Boot;
using BPUA.Application.BusinessLogic;
using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;
using BPUA.Application.TestInfrastructure;
using BPUA.Core;

using Xunit;

namespace BPUA.Application.BootTests
{
    public partial class BPUAPlatformBootstrapperTests
    {
        /// <summary>
        /// Verifies that bootstrap initialization registers the platform services that must exist
        /// even before any external use case assemblies are loaded. In particular, the test checks
        /// the built-in initializing transition handler and the cross-layer routing service.
        /// This confirms that the boot project prepares the minimum internal orchestration backbone
        /// required for the rest of the platform to function.
        /// </summary>
        [Fact]
        public async Task RegistersBuiltInTransitionHandlersAndRouter()
        {
            string appSettingsJson = "{\"PluginFolder\": \"PluginsThatDoNotExistYet\"}";

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson);

            Directory.CreateDirectory(Path.Combine(scope.RootPath, "PluginsThatDoNotExistYet"));

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            await bootstrapper.BootBPUAPlatform(scope.RootPath, true);

            IBPUAApplication application = BPUAApplication.GetInstance();
            IServiceRegistry serviceRegistry = application.ServiceRegistry;

            string initializingApplicationKey = KeyCompiler.CompileTransitionHandlerKey(
                DomainNames.BPUA,
                UseCaseNames.APPLICATION,
                ApplicationLayersNames.BL,
                null,
                TransitionsNames.INITIALIZING_APPLICATION);

            Assert.True(serviceRegistry.TryGetRegisteredType(initializingApplicationKey, out Type registeredTransitionType));
            Assert.Equal(typeof(InitializingApplicationTransitionHandler), registeredTransitionType);

            string routerKey = "RouteTransitionContext/" + typeof(TransitionContextRouter).FullName;
            Assert.True(serviceRegistry.TryGetRegisteredType(routerKey, out Type registeredRouterType));
            Assert.Equal(typeof(TransitionContextRouter), registeredRouterType);
        }
    }
}