using BPUA.Application.Boot;
using BPUA.Application.BootTests.TestInfrastructure;

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
        public void BootBPUAPlatform_Throws_WhenCalledMoreThanOnce()
        {
            string appSettingsJson = """{"PluginFolder": "Plugins"}""";

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson);
            scope.CreateDirectory("Plugins");

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            bootstrapper.BootBPUAPlatform(scope.RootPath, true);

            Assert.ThrowsAny<InvalidOperationException>(delegate
            {
                bootstrapper.BootBPUAPlatform(scope.RootPath, true);
            });
        }
    }
}
