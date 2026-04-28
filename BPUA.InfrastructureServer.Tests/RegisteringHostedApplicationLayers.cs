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
        /// <summary>
        /// Tests the registration of hosted application layers.
        /// </summary>
        [Fact]
        public async Task RegisteringHostedApplicationLayers()
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
            stringBuilder.AppendLine("      \"ApplicationLayerName\": \"SL\"");
//            stringBuilder.AppendLine("      \"StateName\": \"WaitingForApplicationLoad\",");
//            stringBuilder.AppendLine("      \"IsApplicationUseCaseLayer\": \"true\"");
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

            IEnumerable<HostedApplicationLayer> hostedApplicationLayers = bpuaApplication.ServiceRegistry.EnumerateObjectsByType<HostedApplicationLayer>();
            List<HostedApplicationLayer> hostedApplicationLayersList = new List<HostedApplicationLayer>(hostedApplicationLayers);
            Assert.Equal(4, hostedApplicationLayersList.Count);

            bool foundSL = false;
            bool foundBL = false;
            bool foundDPL = false;
            bool foundDAL = false;
            for (int i = 0; i < hostedApplicationLayersList.Count; i++)
            {
                HostedApplicationLayer hostedApplicationLayer = hostedApplicationLayersList[i];
                string? applicationLayerName = hostedApplicationLayer.BpuaIdentifier.ApplicationLayerName;
                switch (applicationLayerName)
                {
                    case ApplicationLayersNames.SL:
                        foundSL = true;
                        Assert.Equal(HostedApplicationLayerState.NotInitialized, hostedApplicationLayer.HostedApplicationLayerState);
                        break;
                    case ApplicationLayersNames.BL:
                        foundBL = true;
                        Assert.Equal(HostedApplicationLayerState.NotInitialized, hostedApplicationLayer.HostedApplicationLayerState);
                        break;
                    case ApplicationLayersNames.DPL:
                        foundDPL = true;
                        Assert.Equal(HostedApplicationLayerState.NotInitialized, hostedApplicationLayer.HostedApplicationLayerState);
                        break;
                    case ApplicationLayersNames.DAL:
                        foundDAL = true;
                        Assert.Equal(HostedApplicationLayerState.NotInitialized, hostedApplicationLayer.HostedApplicationLayerState);
                        break;
                    default:
                        Assert.Fail($"Unexpected application layer name: {applicationLayerName}");
                        break;
                }

                Assert.Equal("BPUA", hostedApplicationLayer.BpuaIdentifier.DomainName);
                Assert.Equal("InfrastructureServer", hostedApplicationLayer.BpuaIdentifier.UseCaseName);
                Assert.False(hostedApplicationLayer.IsApplicationUseCaseLayer);
            }

            Assert.True(foundSL);
            Assert.True(foundBL);
            Assert.True(foundDPL);
            Assert.True(foundDAL);
        }
    }
}
