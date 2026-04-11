using BPUA.Application.Boot;
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
        /// Verifies that the bootstrapper preloads the core static BPUA assemblies and passes them
        /// to the registered <c>UseCaseActivator</c>. These assemblies provide the platform's built-in
        /// logic layers and orchestration services, so they must already be available before any
        /// dynamic use case activation begins.
        /// </summary>
        [Fact]
        public void RegistersExpectedStaticAssembliesInServiceRegistry()
        {
            string appSettingsJson = "{\"PluginFolder\": \"Plugins\"}";

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson);
            scope.CreateDirectory("Plugins");

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            bootstrapper.BootBPUAPlatform(scope.RootPath, true);

            IBPUAApplication application = BPUAApplication.GetInstance();
            IServiceRegistry serviceRegistry = application.ServiceRegistry;
            Assert.True(serviceRegistry.HasAssemblyFacet(typeof(BPUA.Application.BusinessLogic.AssemblyReference).Assembly.FullName!, AssemblyFacet.Services));
            Assert.True(serviceRegistry.HasAssemblyFacet(typeof(BPUA.Application.DataAccessLogic.AssemblyReference).Assembly.FullName!, AssemblyFacet.Services));
            Assert.True(serviceRegistry.HasAssemblyFacet(typeof(BPUA.Application.DataProcessingLogic.AssemblyReference).Assembly.FullName!, AssemblyFacet.Services));
            Assert.True(serviceRegistry.HasAssemblyFacet(typeof(BPUA.Application.Orchestration.AssemblyReference).Assembly.FullName!, AssemblyFacet.Services));
        }
    }
}
