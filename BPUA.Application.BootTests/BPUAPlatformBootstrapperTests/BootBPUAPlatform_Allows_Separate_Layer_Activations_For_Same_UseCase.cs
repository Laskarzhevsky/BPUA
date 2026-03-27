using BPUA.Application.Boot;
using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;
using BPUA.Core;

using System.IO;
using System.Text;

using Xunit;

namespace BPUA.Application.BootTests
{
    public partial class BPUAPlatformBootstrapperTests
    {
        [Fact]
        public async Task BootBPUAPlatform_Allows_Separate_Layer_Activations_For_Same_UseCase()
        {
            string buildFolder = FindBuildFolder();
            string appSettingsJson = "{ \"PluginFolder\": \"PluginFolder\" }";
            string appSettingsJsonFilePath = Path.Combine(buildFolder, "appsettings.json");
            File.WriteAllText(appSettingsJsonFilePath, appSettingsJson, Encoding.UTF8);
            Directory.SetCurrentDirectory(buildFolder);

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            bootstrapper.BootBPUAPlatform(buildFolder, true);

            IBPUAApplication application = BPUAApplication.GetInstance();

            IBPUAIdentifier blIdentifier = new BPUAIdentifier();
            blIdentifier.DomainName = DomainNames.BPUA;
            blIdentifier.UseCaseName = BPUA.Account.Contracts.Contract.ACCOUNT;
            blIdentifier.ApplicationLayerName = ApplicationLayersNames.BL;
            blIdentifier.TransitionName = TransitionsNames.INITIALIZING_USE_CASE;
            blIdentifier.Breadcrumbs = "Libraries\\Setup\\Administration";

            UseCaseActivationResult blActivationResult = await application.ActivateUseCaseAsync(blIdentifier);
            Assert.True(blActivationResult.Succeeded, string.Join(System.Environment.NewLine, blActivationResult.Errors));

            IBPUAIdentifier dplIdentifier = new BPUAIdentifier();
            dplIdentifier.DomainName = DomainNames.BPUA;
            dplIdentifier.UseCaseName = BPUA.Account.Contracts.Contract.ACCOUNT;
            dplIdentifier.ApplicationLayerName = ApplicationLayersNames.DPL;
            dplIdentifier.TransitionName = TransitionsNames.INITIALIZING_USE_CASE;
            dplIdentifier.Breadcrumbs = "Libraries\\Setup\\Administration";

            UseCaseActivationResult dplActivationResult = await application.ActivateUseCaseAsync(dplIdentifier);
            Assert.True(dplActivationResult.Succeeded, string.Join(System.Environment.NewLine, dplActivationResult.Errors));
        }
    }
}
