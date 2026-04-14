using BPUA.Application.Boot;
using BPUA.Application.TestInfrastructure;

using Xunit;

namespace BPUA.Application.BootTests
{
    public partial class BPUAPlatformBootstrapperTests
    {
        /// <summary>
        /// Verifies that the bootstrapper rejects a missing or blank executable-root argument.
        /// The path supplied by the host is part of the bootstrap contract because production
        /// plugin resolution is based on it and because accepting an empty value would hide
        /// a host-startup bug until a later and less understandable failure point.
        /// </summary>
        /// <param name="pathToFolderWithExecutableFile">Invalid executable-root value under test.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task BootBPUAPlatform_Throws_WhenExecutablePathIsNullEmptyOrWhitespace(string? pathToFolderWithExecutableFile)
        {
            string appSettingsJson = """{"PluginFolder": "Plugins"}""";

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson);
            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();

            await Assert.ThrowsAnyAsync<ArgumentException>(async () =>
            {
                await bootstrapper.BootBPUAPlatform(pathToFolderWithExecutableFile!, false);
            });
        }
    }
}
