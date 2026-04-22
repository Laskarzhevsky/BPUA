using BPUA.Application.Boot;
using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;
using BPUA.Application.TestInfrastructure;

using Xunit;

using System.Text;

namespace BPUA.Application.BootTests
{
    public partial class HRApplicationLayersLoadingTests
    {
        [Fact]
        public async Task LoadHRApplicationBusinessLogicLayer()
        {
            string buildFolder = Helpers.FindBuildFolder();
            string pluginFolderPath = Path.Combine(buildFolder, "PluginFolder");
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("  \"PluginFolder\": \"" + Helpers.EscapeJson(pluginFolderPath) + "\",");
            stringBuilder.AppendLine("  \"HostedApplicationLayers\":");
            stringBuilder.AppendLine("  [");
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine("      \"DomainName\": \"HR\",");
            stringBuilder.AppendLine("      \"UseCaseName\": \"Application\",");
            stringBuilder.AppendLine("      \"ApplicationLayerName\": \"BL\"");
            stringBuilder.AppendLine("    }");
            stringBuilder.AppendLine("  ]");
            stringBuilder.AppendLine("}");

            string appSettingsJson = stringBuilder.ToString();

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson);

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            await bootstrapper.BootBPUAPlatform(scope.RootPath, true);

            IBPUAApplication bpuaApplication = BPUAApplication.GetInstance();
            Assert.True(bpuaApplication.ServiceRegistry.TryGetRegisteredObject("HR_Application_BL", out object? value));
            HostedApplicationLayer? hostedApplicationLayer = value as HostedApplicationLayer;
            Assert.NotNull(hostedApplicationLayer);
        }
    }
}
