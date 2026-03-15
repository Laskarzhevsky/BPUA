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
        /// Verifies that in non-development mode the bootstrapper treats the configured
        /// <c>PluginFolder</c> as a path relative to the executable folder supplied by the host.
        /// This is the production deployment behavior: plugins are expected to be located next to,
        /// or underneath, the deployed application rather than the source content root.
        /// </summary>
        [Fact]
        public void InProduction_CombinesExecutableFolderWithPluginFolder()
        {
            string appSettingsJson = "{\"PluginFolder\": \"Plugins\\\\Runtime\"}";

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson);
            string executableRoot = scope.CreateDirectory("ExecutableRoot");
            Directory.CreateDirectory(Path.Combine(executableRoot, "Plugins", "Runtime"));

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            bootstrapper.BootBPUAPlatform(executableRoot, false);

            IBPUAApplication application = BPUAApplication.GetInstance();
            string expectedPath = Path.GetFullPath(Path.Combine(executableRoot, "Plugins", "Runtime"));

            Assert.Equal(expectedPath, application.PathToFolderWithDynamicAssemblies);
        }
    }
}
