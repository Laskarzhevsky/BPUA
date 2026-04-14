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
        /// Verifies that boot still succeeds when <c>appsettings.{Environment}.json</c> is absent.
        /// The environment-specific file is intentionally optional in configuration loading, so the
        /// bootstrapper should fall back to the base <c>appsettings.json</c> values rather than fail.
        /// This protects simple deployments that rely only on the base configuration file.
        /// </summary>
        [Fact]
        public async Task BootBPUAPlatform_WhenEnvironmentSpecificSettingsFileIsMissing_UsesBaseConfiguration()
        {
            string appSettingsJson = """{"PluginFolder": "Plugins/BaseOnly"}""";

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson, "Staging");
            scope.CreateDirectory(Path.Combine("Plugins", "BaseOnly"));

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            await bootstrapper.BootBPUAPlatform(scope.RootPath, true);

            IBPUAApplication application = BPUAApplication.GetInstance();
            string expectedPath = Path.GetFullPath(Path.Combine(scope.RootPath, "Plugins", "BaseOnly"));

            Assert.Equal(expectedPath, application.PathToFolderWithDynamicAssemblies);
        }
    }
}
