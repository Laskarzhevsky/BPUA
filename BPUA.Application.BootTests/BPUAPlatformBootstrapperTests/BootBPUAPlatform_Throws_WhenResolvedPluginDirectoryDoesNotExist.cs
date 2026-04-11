using BPUA.Application.Boot;
using BPUA.Application.TestInfrastructure;

using Xunit;

namespace BPUA.Application.BootTests
{
    public partial class BPUAPlatformBootstrapperTests
    {
        /// <summary>
        /// Verifies that boot fails fast when the configured plugin folder resolves to a directory
        /// that does not exist on disk. A clear startup exception is preferable here because the
        /// platform cannot discover dynamic use-case assemblies from a missing location and should
        /// not continue in a partially booted state that hides the deployment problem.
        /// </summary>
        [Fact]
        public void BootBPUAPlatform_Throws_WhenResolvedPluginDirectoryDoesNotExist()
        {
            string appSettingsJson = """{"PluginFolder": "Plugins/Missing"}""";

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson);
            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();

            DirectoryNotFoundException exception = Assert.Throws<DirectoryNotFoundException>(delegate
            {
                bootstrapper.BootBPUAPlatform(scope.RootPath, true);
            });

            Assert.Contains("Plugins", exception.Message, StringComparison.OrdinalIgnoreCase);
        }
    }
}
