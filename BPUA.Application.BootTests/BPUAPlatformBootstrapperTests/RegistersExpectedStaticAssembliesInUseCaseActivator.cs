using System.Reflection;

using BPUA.Application.Boot;
using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;
using BPUA.Application.BootTests.TestInfrastructure;

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
        public void RegistersExpectedStaticAssembliesInUseCaseActivator()
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

            Assert.Contains(typeof(BPUA.Application.BusinessLogic.AssemblyReference).Assembly, useCaseActivator.ListOfLoadedAssemblies);
            Assert.Contains(typeof(BPUA.Application.DataAccessLogic.AssemblyReference).Assembly, useCaseActivator.ListOfLoadedAssemblies);
            Assert.Contains(typeof(BPUA.Application.DataProcessingLogic.AssemblyReference).Assembly, useCaseActivator.ListOfLoadedAssemblies);
            Assert.Contains(typeof(BPUA.Application.Orchestration.AssemblyReference).Assembly, useCaseActivator.ListOfLoadedAssemblies);

            int distinctKnownAssemblies = useCaseActivator.ListOfLoadedAssemblies
                .Where(delegate(Assembly assembly)
                {
                    return assembly == typeof(BPUA.Application.BusinessLogic.AssemblyReference).Assembly
                        || assembly == typeof(BPUA.Application.DataAccessLogic.AssemblyReference).Assembly
                        || assembly == typeof(BPUA.Application.DataProcessingLogic.AssemblyReference).Assembly
                        || assembly == typeof(BPUA.Application.Orchestration.AssemblyReference).Assembly;
                })
                .Distinct()
                .Count();

            Assert.Equal(4, distinctKnownAssemblies);
        }
    }
}
