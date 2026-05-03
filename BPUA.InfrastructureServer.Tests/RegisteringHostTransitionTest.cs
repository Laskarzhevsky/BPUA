using BPUA.Application.Boot;
using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;
using BPUA.Application.TestInfrastructure;
using BPUA.Core;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.Extensions;
using PocoDataSet.IData;

using System.Text;

using Xunit;

namespace BPUA.InfrastructureServer.Tests
{
    public partial class InfrastructureServerTests
    {
        /// <summary>
        /// Tests the registration of hosted application layers.
        /// </summary>
        [Fact]
        public async Task RegisteringHostTransitionTest()
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

            // The remote host will send a request to the BPUA application to register itself as a host for the specified application layer.
            // This will trigger the execution of the transition that registers the hosted application layer.
            IBPUAIdentifier bpuaIdentifier = BPUA.InfrastructureServer.Contracts.Endpoints.RegisteringHost();

            IDataSet dataSet = DataSetFactory.CreateDataSet();
            dataSet.AddRequestMetadata(bpuaIdentifier);
            RouteTransitionContextEventArgs routeTransitionContextEventArgs = new RouteTransitionContextEventArgs(dataSet);
            ServiceRequestEventArgs serviceRequestEventArgs = new ServiceRequestEventArgs("SendRequestToNextHandler", routeTransitionContextEventArgs);

            IBPUAApplication bpuaApplication = BPUAApplication.GetInstance();
            await bpuaApplication.RequestHandler_RequestServiceEvent(null, serviceRequestEventArgs);

            UseCaseActivationResult useCaseActivationResult = await ((BPUAApplication)bpuaApplication).ActivateUseCaseAsync(bpuaIdentifier);
            Assert.True(useCaseActivationResult.Succeeded, string.Join(System.Environment.NewLine, useCaseActivationResult.Errors));

            IServiceRegistry serviceRegistry = bpuaApplication.ServiceRegistry;

            string stateHandlerKey = KeyCompiler.CompileStateHandlerKey(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName, bpuaIdentifier.StateName);
            IStateHandler? stateHandler = BPUAApplication.GetInstance().GetRequestHandler(stateHandlerKey) as IStateHandler;

        }
    }
}
