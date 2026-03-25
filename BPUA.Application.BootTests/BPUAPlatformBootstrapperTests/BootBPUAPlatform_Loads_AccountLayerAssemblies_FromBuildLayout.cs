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
        /// Verifies that the bootstrapper can load the Account use-case assemblies from the real
        /// shared Build layout produced by MSBuild. The test reads the path inputs from
        /// SharedBuildSettings.props and the Account project file instead of hardcoding the plugin
        /// directory structure in test code.
        /// </summary>
        [Fact]
        public void BootBPUAPlatform_Loads_AccountLayerAssemblies_FromBuildLayout()
        {
            string accountPluginFolder = BuildLayoutResolver.ResolvePluginFolderForProject("BPUA.Account.BusinessLogic");

            Assert.True(Directory.Exists(accountPluginFolder), "Account plugin folder was not found: " + accountPluginFolder);

            string appSettingsJson ="{ \"PluginFolder\": \"" + Helpers.EscapeJson(accountPluginFolder) + "\" }";

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson);

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            bootstrapper.BootBPUAPlatform(scope.RootPath, true);

            IBPUAApplication application = BPUAApplication.GetInstance();
            IServiceRegistry serviceRegistry = application.ServiceRegistry;

            object? registeredObject;
            bool found = serviceRegistry.TryGetRegisteredObject(typeof(IUseCaseActivator).Name, out registeredObject);

            Assert.True(found);

            UseCaseActivator useCaseActivator = Assert.IsType<UseCaseActivator>(registeredObject);

            Assert.Contains(useCaseActivator.ListOfLoadedAssemblies, delegate (Assembly assembly)
            {
                return assembly.GetName().Name == "BPUA.Account.BusinessLogic";
            });

            Assert.Contains(useCaseActivator.ListOfLoadedAssemblies, delegate (Assembly assembly)
            {
                return assembly.GetName().Name == "BPUA.Account.DataAccessLogic";
            });

            Assert.Contains(useCaseActivator.ListOfLoadedAssemblies, delegate (Assembly assembly)
            {
                return assembly.GetName().Name == "BPUA.Account.DataProcessingLogic";
            });
        }
    }
}
