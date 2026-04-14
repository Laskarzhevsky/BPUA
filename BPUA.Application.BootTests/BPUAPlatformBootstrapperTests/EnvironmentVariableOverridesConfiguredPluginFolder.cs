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
        /// Verifies that environment variables participate in configuration composition and can
        /// override the <c>PluginFolder</c> value from JSON files. This matters because deployed
        /// hosts often redirect plugin locations without changing packaged configuration files,
        /// and the bootstrapper relies on <c>AddEnvironmentVariables()</c> for that last-mile override.
        /// </summary>
        [Fact]
        public async Task EnvironmentVariableOverridesConfiguredPluginFolder()
        {
            string appSettingsJson = "{\"PluginFolder\": \"Plugins/Base\"}";

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson);
            scope.SetEnvironmentVariable("PluginFolder", "Plugins/EnvOverride");
            scope.CreateDirectory(Path.Combine("Plugins", "EnvOverride"));

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            await bootstrapper.BootBPUAPlatform(scope.RootPath, true);

            IBPUAApplication application = BPUAApplication.GetInstance();
            string expectedPath = Path.GetFullPath(Path.Combine(scope.RootPath, "Plugins", "EnvOverride"));

            Assert.Equal(expectedPath, application.PathToFolderWithDynamicAssemblies);
        }
    }
}
