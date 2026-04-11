using BPUA.Application.Boot;
using BPUA.Application.TestInfrastructure;

using Xunit;

namespace BPUA.Application.BootTests
{
    public partial class BPUAPlatformBootstrapperTests
    {
        /// <summary>
        /// Verifies that boot fails fast when the required <c>PluginFolder</c> setting is missing.
        /// Without this setting the platform cannot determine where dynamic use case assemblies live,
        /// so silently continuing would produce a half-initialized runtime. The expected exception
        /// documents that this configuration value is mandatory.
        /// </summary>
        [Fact]
        public void BootBPUAPlatform_Throws_WhenPluginFolderSettingIsMissing()
        {
            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope("{}");
            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();

            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                bootstrapper.BootBPUAPlatform(scope.RootPath, false);
            });

            Assert.Contains("PluginFolder", exception.Message, StringComparison.Ordinal);
        }
    }
}
