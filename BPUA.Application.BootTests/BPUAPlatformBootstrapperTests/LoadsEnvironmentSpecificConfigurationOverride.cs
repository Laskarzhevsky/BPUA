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
        /// Verifies that <c>appsettings.{Environment}.json</c> overrides the base configuration
        /// when <c>ASPNETCORE_ENVIRONMENT</c> is defined. The test proves that boot does not read
        /// only the base <c>appsettings.json</c>, but correctly honors environment-specific settings,
        /// which is important when plugin locations differ across local, test, staging, and production.
        /// </summary>
        [Fact]
        public void LoadsEnvironmentSpecificConfigurationOverride()
        {
            string appSettingsJson = "{\"PluginFolder\": \"Plugins/Base\"}";
            string environmentSpecificJson = "{\"PluginFolder\": \"Plugins/Tests\"}";

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson, "Integration", environmentSpecificJson);
            scope.CreateDirectory(Path.Combine("Plugins", "Tests"));

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            bootstrapper.BootBPUAPlatform(scope.RootPath, true);

            IBPUAApplication application = BPUAApplication.GetInstance();
            string expectedPath = Path.GetFullPath(Path.Combine(scope.RootPath, "Plugins", "Tests"));

            Assert.Equal(expectedPath, application.PathToFolderWithDynamicAssemblies);
        }
    }
}
