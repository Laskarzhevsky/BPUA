using BPUA.Application.Boot;
using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;
using BPUA.Core;

using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

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
            IServiceRegistry serviceRegistry = application.ServiceRegistry;

            IBPUAIdentifier businessLogicIdentifier = CreateIdentifier(BPUA.Application.Contracts.ApplicationLayersNames.BL);
            UseCaseActivationResult businessLogicResult = await application.ActivateUseCaseAsync(businessLogicIdentifier);

            Assert.True(businessLogicResult.Succeeded);
            Assert.False(businessLogicResult.NoAdditionalAssembliesWereLoaded);

            IBPUAIdentifier dataProcessingLogicIdentifier = CreateIdentifier(BPUA.Application.Contracts.ApplicationLayersNames.DPL);
            UseCaseActivationResult dataProcessingLogicResult = await application.ActivateUseCaseAsync(dataProcessingLogicIdentifier);

            Assert.True(dataProcessingLogicResult.Succeeded);
            Assert.False(dataProcessingLogicResult.NoAdditionalAssembliesWereLoaded);

            object? registeredObject;
            bool found = serviceRegistry.TryGetRegisteredObject(typeof(IUseCaseActivator).Name, out registeredObject);

            Assert.True(found);

            UseCaseActivator useCaseActivator = Assert.IsType<UseCaseActivator>(registeredObject);

            Assert.Contains(useCaseActivator.ListOfLoadedAssemblies, delegate (Assembly assembly)
            {
                return assembly.GetName().Name == "BPUA.Account.BL";
            });

            Assert.Contains(useCaseActivator.ListOfLoadedAssemblies, delegate (Assembly assembly)
            {
                return assembly.GetName().Name == "BPUA.Account.DPL";
            });
        }

        static IBPUAIdentifier CreateIdentifier(string applicationLayerName)
        {
            IBPUAIdentifier bpuaIdentifier = new BPUAIdentifier();
            bpuaIdentifier.DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
            bpuaIdentifier.UseCaseName = BPUA.Account.Contracts.Contract.ACCOUNT;
            bpuaIdentifier.ApplicationLayerName = applicationLayerName;
            bpuaIdentifier.StateName = default!;
            bpuaIdentifier.TransitionName = BPUA.Application.Contracts.TransitionsNames.INITIALIZING_USE_CASE;
            bpuaIdentifier.Breadcrumbs = "Libraries\\Setup\\Administration";
            return bpuaIdentifier;
        }
    }
}
