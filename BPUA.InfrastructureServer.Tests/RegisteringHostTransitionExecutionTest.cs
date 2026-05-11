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
        /// Tests the execution of the transition that registers hosted application layers with BPUA Infrastructure Server.
        /// </summary>
        [Fact]
        public async Task RegisteringHostTransitionExecutionTest()
        {
            // Boot BPUA platform
            string buildFolder = Helpers.FindBuildFolder();
            BpuaPlatformBootstrapper bootstrapper = new BpuaPlatformBootstrapper();
            await bootstrapper.BootBpuaPlatform(buildFolder);

            // Testing the transition that registers hosted application layers.
            // This transition is triggered when a remote host (caller) sends a request to the BPUA application to register itself as a host for a specified application layer.

            // 1. The remote host (caller) needs to identify itself. Simulating this by creating a dummy BPU identifier
            IBpuIdentifier remoteHostBpuIdentifier = new BpuIdentifier("HR", "Acounting", "DPL", "State1", "Transition1");

            // 2. The remote host will send a request to the BPUA application to register itself as a host of the specific application layer.
            // This will trigger the execution of the transition that registers the hosted application layer.
            IBpuIdentifier bpuIdentifier = BPUA.InfrastructureServer.Contracts.EndpointIdentifiers.RegisteringHost;

            IDataSet requestTransitionContext = DataSetFactory.CreateDataSet();
            requestTransitionContext.AddRequestMetadata(remoteHostBpuIdentifier);
            requestTransitionContext.AddRequestMetadata(bpuIdentifier);

            // 3. Provide the details of the hosted application layer in the request data set.
            IDataTable dataTable = requestTransitionContext.AddNewTableFromPocoInterface(typeof(IHostedApplicationLayer).Name, typeof(IHostedApplicationLayer));
            IHostedApplicationLayer? hostedApplicationLayer = dataTable.AddNewRow<IHostedApplicationLayer>();
            hostedApplicationLayer!.DomainName = "HR";
            hostedApplicationLayer.UseCaseName = "Accounting";
            hostedApplicationLayer.ApplicationLayerName = "DPL";

            hostedApplicationLayer = dataTable.AddNewRow<IHostedApplicationLayer>();
            hostedApplicationLayer!.DomainName = "HR";
            hostedApplicationLayer.UseCaseName = "Accounting";
            hostedApplicationLayer.ApplicationLayerName = "DAL";

            // 4. Send the request to the BPUA application.
            RouteTransitionContextEventArgs routeTransitionContextEventArgs = new RouteTransitionContextEventArgs(requestTransitionContext);
            ServiceRequestEventArgs serviceRequestEventArgs = new ServiceRequestEventArgs(routeTransitionContextEventArgs);

            IBpuaApplication bpuaApplication = BpuaApplication.GetInstance();
            await bpuaApplication.RequestHandler_RequestServiceEvent(null, serviceRequestEventArgs);

            // To be continued: Verify that the hosted application layer was registered successfully.
            // This can be done by checking the state of the BPUA application or by sending another request
            // to retrieve the list of registered hosted application layers and verifying that the new host is included in the list.
        }
    }
}
