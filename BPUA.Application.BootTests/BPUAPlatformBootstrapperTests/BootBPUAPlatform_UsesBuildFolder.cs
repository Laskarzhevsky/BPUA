using System;
using System.IO;
using System.Reflection;

using BPUA.Application.Boot;
using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;
using BPUA.Application.BootTests.TestInfrastructure;

using Xunit;

namespace BPUA.Application.BootTests
{
    public partial class BPUAPlatformBootstrapperTests
    {
        [Fact]
        public void BootBPUAPlatform_Loads_AccountLayerAssemblies_From_Build_PluginFolder()
        {
            string buildFolder = FindBuildFolder();
            string accountPluginFolder = Path.Combine(buildFolder, "PluginFolder", "BPUA.Account");

            Assert.True(Directory.Exists(accountPluginFolder), "Account plugin folder was not found: " + accountPluginFolder);

            string appSettingsJson ="{ \"PluginFolder\": \"" + Helpers.EscapeJson(accountPluginFolder) + "\" }";

            using TestBootstrapEnvironmentScope scope = new TestBootstrapEnvironmentScope(appSettingsJson);

            BPUAPlatformBootstrapper bootstrapper = new BPUAPlatformBootstrapper();
            bootstrapper.BootBPUAPlatform(scope.RootPath, true);

            IBPUAApplication application = BPUAApplication.GetInstance();
            IServiceRegistry serviceRegistry = application.ServiceRegistry;

            object? registeredObject;
            bool found = serviceRegistry.TryGetRegisteredObject(typeof(IUseCaseActivator).Name, out registeredObject);

            Assert.True(found);

            UseCaseActivator useCaseActivator = Assert.IsType<UseCaseActivator>(registeredObject);

            Assert.Contains(useCaseActivator.ListOfLoadedAssemblies, delegate (Assembly assembly)
            {
                return assembly.GetName().Name == "BPUA.Account.BusinessLogic";
            });

            Assert.Contains(useCaseActivator.ListOfLoadedAssemblies, delegate (Assembly assembly)
            {
                return assembly.GetName().Name == "BPUA.Account.DataAccessLogic";
            });

            Assert.Contains(useCaseActivator.ListOfLoadedAssemblies, delegate (Assembly assembly)
            {
                return assembly.GetName().Name == "BPUA.Account.DataProcessingLogic";
            });
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