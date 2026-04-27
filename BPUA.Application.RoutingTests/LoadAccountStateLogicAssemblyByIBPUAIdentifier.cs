using BPUA.Application.Boot;
using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;
using BPUA.Core;
using BPUA.Application.TestInfrastructure;

using Xunit;

using System.Text;

namespace BPUA.Application.RoutingTests
{
    public partial class BPUAPlatformRoutingTests
    {
        [Fact]
        public async Task LoadAccountStateLogicAssemblyByIBPUAIdentifier()
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
            stringBuilder.AppendLine("      \"ApplicationLayerName\": \"SL\",");
            stringBuilder.AppendLine("      \"StateName\": \"WaitingForApplicationLoad\"");
            stringBuilder.AppendLine("    },");
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine("      \"DomainName\": \"HR\",");
            stringBuilder.AppendLine("      \"UseCaseName\": \"Application\",");
            stringBuilder.AppendLine("      \"ApplicationLayerName\": \"BL\",");
            stringBuilder.AppendLine("    },");
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine("      \"DomainName\": \"HR\",");
            stringBuilder.AppendLine("      \"UseCaseName\": \"Application\",");
            stringBuilder.AppendLine("      \"ApplicationLayerName\": \"DPL\",");
            stringBuilder.AppendLine("    },");
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine("      \"DomainName\": \"HR\",");
            stringBuilder.AppendLine("      \"UseCaseName\": \"Application\",");
            stringBuilder.AppendLine("      \"ApplicationLayerName\": \"DAL\",");
            stringBuilder.AppendLine("    }");
            stringBuilder.AppendLine("  ]");
            stringBuilder.AppendLine("}");

            string appSettingsJson = stringBuilder.ToString();

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson);

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            await bootstrapper.BootBPUAPlatform(scope.RootPath, true);

            IBPUAApplication bpuaApplication = BPUAApplication.GetInstance();

            IBPUAIdentifier bpuaIdentifier = new BPUAIdentifier();
            bpuaIdentifier.DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
            bpuaIdentifier.UseCaseName = BPUA.Account.Contracts.UseCaseName.ACCOUNT;
            bpuaIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.SL;
            bpuaIdentifier.StateName = BPUA.Application.Contracts.StateNames.INITIAL;

            UseCaseActivationResult useCaseActivationResult = await bpuaApplication.ActivateUseCaseAsync(bpuaIdentifier);
            Assert.True(useCaseActivationResult.Succeeded, string.Join(System.Environment.NewLine, useCaseActivationResult.Errors));

            IServiceRegistry serviceRegistry = bpuaApplication.ServiceRegistry;

            string stateHandlerKey = KeyCompiler.CompileStateHandlerKey(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName, bpuaIdentifier.StateName);
            IStateHandler? stateHandler = BPUAApplication.GetInstance().GetRequestHandler(stateHandlerKey) as IStateHandler;

            Assert.NotNull(stateHandler);
            Assert.True(serviceRegistry.HasAssemblyFacet("BPUA.Account.SL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AssemblyFacet.Services));
            Assert.False(serviceRegistry.HasAssemblyFacet("BPUA.Account.BL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AssemblyFacet.Services));
            Assert.False(serviceRegistry.HasAssemblyFacet("BPUA.Account.DAL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AssemblyFacet.Services));
            Assert.False(serviceRegistry.HasAssemblyFacet("BPUA.Account.DPL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AssemblyFacet.Services));
        }
    }
}
