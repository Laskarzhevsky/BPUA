using BPUA.Application.Boot;
using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;
using BPUA.Application.TestInfrastructure;

using Xunit;

using System.Text;

namespace BPUA.InfrastructureServer.Tests
{
    public class InfrastructureServerTests
    {
        [Fact]
        public async Task InitializingApplication()
        {
            string buildFolder = Helpers.FindBuildFolder();
            string pluginFolderPath = Path.Combine(buildFolder, "PluginFolder");
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("  \"PluginFolder\": \"" + Helpers.EscapeJson(pluginFolderPath) + "\",");
            stringBuilder.AppendLine("  \"HostedApplicationLayers\":");
            stringBuilder.AppendLine("  [");
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine("      \"DomainName\": \"BPUA\",");
            stringBuilder.AppendLine("      \"UseCaseName\": \"InfrastructureServer\",");
            stringBuilder.AppendLine("      \"ApplicationLayerName\": \"SL\",");
            stringBuilder.AppendLine("      \"StateName\": \"WaitingForApplicationLoad\"");
            stringBuilder.AppendLine("    },");
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine("      \"DomainName\": \"BPUA\",");
            stringBuilder.AppendLine("      \"UseCaseName\": \"InfrastructureServer\",");
            stringBuilder.AppendLine("      \"ApplicationLayerName\": \"BL\"");
            stringBuilder.AppendLine("    },");
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine("      \"DomainName\": \"BPUA\",");
            stringBuilder.AppendLine("      \"UseCaseName\": \"InfrastructureServer\",");
            stringBuilder.AppendLine("      \"ApplicationLayerName\": \"DPL\"");
            stringBuilder.AppendLine("    },");
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine("      \"DomainName\": \"BPUA\",");
            stringBuilder.AppendLine("      \"UseCaseName\": \"InfrastructureServer\",");
            stringBuilder.AppendLine("      \"ApplicationLayerName\": \"DAL\"");
            stringBuilder.AppendLine("    }");
            stringBuilder.AppendLine("  ]");
            stringBuilder.AppendLine("}");

            string appSettingsJson = stringBuilder.ToString();

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson);

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            await bootstrapper.BootBPUAPlatform(scope.RootPath, true);

            IBPUAApplication bpuaApplication = BPUAApplication.GetInstance();

        }
    }
}
