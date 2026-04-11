using BPUA.Application.Boot;
using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;
using BPUA.Core;
using BPUA.Application.TestInfrastructure;

using Xunit;

namespace BPUA.Application.RoutingTests
{
    public partial class BPUAPlatformRoutingTests
    {
        [Fact]
        public async Task Loads_Account_StateLogic_Assembly()
        {
            string buildFolder = Helpers.FindBuildFolder();
            string pluginFolderPath = Path.Combine(buildFolder, "PluginFolder");
            string appSettingsJson = "{ \"PluginFolder\": \"" + Helpers.EscapeJson(pluginFolderPath) + "\" }";

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson);

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            bootstrapper.BootBPUAPlatform(scope.RootPath, true);

            IBPUAApplication bpuaApplication = BPUAApplication.GetInstance();

            // Blazor application navigates to the /BPUA_Account_SL_Initial (current path) page
            // During OnInitializedAsync handling, page get state handler by using BPUAServiceLocator.GetBPUAService(CurrentPath);
            ContentPageMock contentPageMock = new ContentPageMock("/BPUA_Account_SL_Initial", bpuaApplication);
            await contentPageMock.OnInitializedAsync();

            IBPUAIdentifier bpuaIdentifier = new BPUAIdentifier();
            bpuaIdentifier.DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
            bpuaIdentifier.UseCaseName = BPUA.Account.Contracts.Contract.ACCOUNT;
            bpuaIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.SL;
            bpuaIdentifier.StateName = BPUA.Application.Contracts.StateNames.INITIAL;
            bpuaIdentifier.Breadcrumbs = "Libraries\\Setup\\Administration";

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
