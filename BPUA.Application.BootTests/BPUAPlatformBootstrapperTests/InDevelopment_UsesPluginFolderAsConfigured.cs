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
        /// Verifies that in development mode the bootstrapper uses the <c>PluginFolder</c>
        /// value exactly as configured in application settings, resolving it relative to the
        /// current content root rather than rebasing it to a runtime executable folder.
        /// This protects the expected developer workflow where plugins live under the solution
        /// or web application root during local execution.
        /// </summary>
        [Fact]
        public void InDevelopment_UsesPluginFolderAsConfigured()
        {
            string appSettingsJson = "{\"PluginFolder\": \"Plugins/DevRoot\"}";

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson);
            scope.CreateDirectory(Path.Combine("Plugins", "DevRoot"));

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            bootstrapper.BootBPUAPlatform(scope.RootPath, true);

            IBPUAApplication application = BPUAApplication.GetInstance();
            string expectedPath = Path.GetFullPath(Path.Combine(scope.RootPath, "Plugins", "DevRoot"));

            Assert.Equal(expectedPath, application.PathToFolderWithDynamicAssemblies);
        }
    }
}
