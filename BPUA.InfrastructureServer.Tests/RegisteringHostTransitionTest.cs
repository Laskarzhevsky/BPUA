using BPUA.Application.Boot;
using BPUA.Application.Contracts;
using BPUA.Application.NonFunctionalContracts;
using BPUA.Application.Orchestration;
using BPUA.Application.TestInfrastructure;
using BPUA.Core;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.Extensions;
using PocoDataSet.IData;

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

            string appSettingsJsonFilePath = Path.Combine(buildFolder, "bpua.host.appsettings.json");
            string appSettingsJson = File.ReadAllText(appSettingsJsonFilePath);

            string appSettingsJsonSchemaFilePath = Path.Combine(buildFolder, "bpua.host.appsettings.schema.json");
            string appSettingsJsonSchemaJson = File.ReadAllText(appSettingsJsonSchemaFilePath);

//            string pluginFolderPath = Path.Combine(buildFolder, "PluginFolder");

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson, null, null, appSettingsJsonSchemaJson);

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            await bootstrapper.BootBPUAPlatform(buildFolder, true);

            // The remote host (caller) needs to identify itself. Simulating this by creating a dummy BPUA identifier
            IBPUAIdentifier remoteHostBpuiIdentifier = new BPUAIdentifier("HR", "Acounting", "DPL", "State1", "Transition1");

            // The remote host will send a request to the BPUA application to register itself as a host for the specified application layer.
            // This will trigger the execution of the transition that registers the hosted application layer.
            IBPUAIdentifier bpuaIdentifier = BPUA.InfrastructureServer.Contracts.Endpoints.RegisteringHost();

            IDataSet dataSet = DataSetFactory.CreateDataSet();
            dataSet.AddRequestMetadata(remoteHostBpuiIdentifier);
            dataSet.AddRequestMetadata(bpuaIdentifier);

            IDataTable dataTable = dataSet.AddNewTableFromPocoInterface(typeof(IHostedApplicationLayer).Name, typeof(IHostedApplicationLayer));
            IHostedApplicationLayer? hostedApplicationLayer = dataTable.AddNewRow<IHostedApplicationLayer>();
            hostedApplicationLayer!.DomainName = "HR";
            hostedApplicationLayer.UseCaseName = "Accounting";
            hostedApplicationLayer.ApplicationLayerName = "DPL";

            RouteTransitionContextEventArgs routeTransitionContextEventArgs = new RouteTransitionContextEventArgs(dataSet);
            ServiceRequestEventArgs serviceRequestEventArgs = new ServiceRequestEventArgs("SendRequestToNextHandler", routeTransitionContextEventArgs);

            IBPUAApplication bpuaApplication = BPUAApplication.GetInstance();
            await bpuaApplication.RequestHandler_RequestServiceEvent(null, serviceRequestEventArgs);
            /*
            UseCaseActivationResult useCaseActivationResult = await ((BPUAApplication)bpuaApplication).ActivateUseCaseAsync(bpuaIdentifier);
            Assert.True(useCaseActivationResult.Succeeded, string.Join(System.Environment.NewLine, useCaseActivationResult.Errors));

            IServiceRegistry serviceRegistry = bpuaApplication.ServiceRegistry;

            string stateHandlerKey = KeyCompiler.CompileStateHandlerKey(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName, bpuaIdentifier.StateName);
            IStateHandler? stateHandler = BPUAApplication.GetInstance().GetRequestHandler(stateHandlerKey) as IStateHandler;
            */
        }
    }
}
