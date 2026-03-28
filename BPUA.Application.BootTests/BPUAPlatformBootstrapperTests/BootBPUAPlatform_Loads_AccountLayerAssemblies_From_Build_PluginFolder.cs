using BPUA.Application.Boot;
using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;
using BPUA.Core;

using System.Text;

using Xunit;

namespace BPUA.Application.BootTests
{
    public partial class BPUAPlatformBootstrapperTests
    {
        [Fact]
        public async Task BootBPUAPlatform_Loads_AccountLayerAssemblies_From_Build_PluginFolder()
        {
            string buildFolder = FindBuildFolder();
            string appSettingsJson = "{ \"PluginFolder\": \"PluginFolder\" }";
            string appSettingsJsonFilePath = Path.Combine(buildFolder, "appsettings.json");
            File.WriteAllText(appSettingsJsonFilePath, appSettingsJson, Encoding.UTF8);
            Directory.SetCurrentDirectory(buildFolder);

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            bootstrapper.BootBPUAPlatform(buildFolder, true);

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

        static string FindBuildFolder()
        {
            string current = AppContext.BaseDirectory;

            while (!string.IsNullOrWhiteSpace(current))
            {
                string candidate = Path.Combine(current, "Build");
                if (Directory.Exists(candidate))
                {
                    return candidate;
                }

                DirectoryInfo? parent = Directory.GetParent(current);
                current = parent == null ? string.Empty : parent.FullName;
            }

            throw new DirectoryNotFoundException("Build folder not found.");
        }

    }
}