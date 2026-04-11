using BPUA.Application.Boot;
using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;
using BPUA.Application.TestInfrastructure;

using Xunit;

namespace BPUA.Application.BootTests
{
    public partial class BPUAPlatformBootstrapperTests
    {
        /// <summary>
        /// Verifies that the bootstrapper registers an <c>IUseCaseActivator</c> instance into the
        /// application service registry and populates it with the information gathered during boot.
        /// The activator is expected to receive the loaded static assemblies, the assembly processors,
        /// and the resolved plugin folder so that later use case activation can continue from the
        /// exact runtime context established by the bootstrap phase.
        /// </summary>
        [Fact]
        public void RegistersUseCaseActivatorWithProcessorsAndPluginPath()
        {
            string appSettingsJson = "{\"PluginFolder\": \"Plugins\"}";

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson);
            scope.CreateDirectory("Plugins");

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            bootstrapper.BootBPUAPlatform(scope.RootPath, true);

            IBPUAApplication application = BPUAApplication.GetInstance();
            IServiceRegistry serviceRegistry = application.ServiceRegistry;
            object? registeredObject;
            bool found = serviceRegistry.TryGetRegisteredObject(typeof(IUseCaseActivator).Name, out registeredObject);

            Assert.True(found);
            UseCaseActivator useCaseActivator = Assert.IsType<UseCaseActivator>(registeredObject);
            Assert.NotNull(useCaseActivator.ListOfAssemblyProcessors);
            Assert.NotEmpty(useCaseActivator.ListOfAssemblyProcessors);
            Assert.Equal(application.PathToFolderWithDynamicAssemblies, useCaseActivator.PathToFolderWithDynamicAssemblies);
        }
    }
}
