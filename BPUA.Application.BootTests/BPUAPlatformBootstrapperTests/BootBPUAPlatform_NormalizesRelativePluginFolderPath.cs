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
        /// Verifies that the bootstrapper normalizes relative plugin paths before storing them in
        /// application state. This matters because configuration may contain <c>.</c> or <c>..</c>
        /// segments, yet downstream code should receive one stable absolute path instead of having
        /// to interpret every relative variant on its own.
        /// </summary>
        [Fact]
        public async Task BootBPUAPlatform_NormalizesRelativePluginFolderPath()
        {
            string appSettingsJson = """{"PluginFolder": "./Plugins/../Plugins/Normalized"}""";

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson);
            scope.CreateDirectory(Path.Combine("Plugins", "Normalized"));

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            await bootstrapper.BootBPUAPlatform(scope.RootPath, true);

            IBPUAApplication application = BPUAApplication.GetInstance();
            string expectedPath = Path.GetFullPath(Path.Combine(scope.RootPath, "Plugins", "Normalized"));

            Assert.Equal(expectedPath, application.PathToFolderWithDynamicAssemblies);
        }
    }
}
