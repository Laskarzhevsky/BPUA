using BPUA.Application.Boot;
using BPUA.Application.TestInfrastructure;

using Xunit;

namespace BPUA.Application.BootTests
{
    public partial class BPUAPlatformBootstrapperTests
    {
        /// <summary>
        /// Verifies that platform boot is treated as a one-time operation.
        /// Re-running the bootstrapper against an already initialized singleton application can
        /// mask startup bugs, duplicate registrations, and produce inconsistent runtime state,
        /// so this test documents the intended contract that a second boot attempt must fail fast.
        /// </summary>
        [Fact]
        public async Task BootBPUAPlatform_Throws_WhenCalledMoreThanOnce()
        {
            string appSettingsJson = """{"PluginFolder": "Plugins"}""";

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson);
            scope.CreateDirectory("Plugins");

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            await bootstrapper.BootBPUAPlatform(scope.RootPath, true);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await bootstrapper.BootBPUAPlatform(scope.RootPath, true);
            });
        }
    }
}
