using BPUA.Application.Boot;
using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;
using BPUA.Core;
using BPUA.Application.TestInfrastructure;

using Xunit;

namespace BPUA.Application.BootTests
{
    public partial class BPUAPlatformBootstrapperTests
    {
        [Fact]
        public async Task Loads_Account_BusinessLogic_Assembly()
        {
            string buildFolder = Helpers.FindBuildFolder();
            string pluginFolderPath = Path.Combine(buildFolder, "PluginFolder");
            string appSettingsJson = "{ \"PluginFolder\": \"" + Helpers.EscapeJson(pluginFolderPath) + "\" }";

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson);

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            await bootstrapper.BootBPUAPlatform(scope.RootPath, true);

            IBPUAApplication application = BPUAApplication.GetInstance();

            IBPUAIdentifier bpuaIdentifier = new BPUAIdentifier();
            bpuaIdentifier.DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
            bpuaIdentifier.UseCaseName = BPUA.Account.Contracts.Contract.ACCOUNT;
            bpuaIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.BL;
            bpuaIdentifier.StateName = default!;
            bpuaIdentifier.TransitionName = BPUA.Application.Contracts.TransitionsNames.INITIALIZING_USE_CASE;
            bpuaIdentifier.Breadcrumbs = "Libraries\\Setup\\Administration";

            UseCaseActivationResult useCaseActivationResult = await application.ActivateUseCaseAsync(bpuaIdentifier);
            Assert.True(useCaseActivationResult.Succeeded, string.Join(System.Environment.NewLine, useCaseActivationResult.Errors));

            IServiceRegistry serviceRegistry = application.ServiceRegistry;

            Assert.True(serviceRegistry.HasAssemblyFacet("BPUA.Account.BL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AssemblyFacet.Services));
            Assert.False(serviceRegistry.HasAssemblyFacet("BPUA.Account.DAL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AssemblyFacet.Services));
            Assert.False(serviceRegistry.HasAssemblyFacet("BPUA.Account.DPL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AssemblyFacet.Services));

        }
    }
}
