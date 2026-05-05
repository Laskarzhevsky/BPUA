using BPUA.Application.Contracts;
using BPUA.Core;

using Microsoft.Extensions.Configuration;

using System;
using System.Threading.Tasks;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Provides hosted application layers initializer functionality
    /// </summary>
    public static partial class HostedApplicationLayersInitializer
    {
        #region Public Methods
        /// <summary>
        /// Initializes hosted application layers
        /// </summary>
        /// <param name="configuration">Application configuration</param>
        /// <param name="bpuaApplication">BPUA application instance</param>
        public static async Task Initialize(IBPUAApplication bpuaApplication)
        {
            HostUrl = ConfigurationReader.GetRequiredValue(bpuaApplication, "HostUrl");

            IConfigurationSection hostedApplicationLayersSection = bpuaApplication.ApplicationConfiguration.GetSection("HostedApplicationLayers");
            if (!hostedApplicationLayersSection.Exists())
            {
                return;
            }

            RegisterHostedApplicationLayers(bpuaApplication, hostedApplicationLayersSection, HostUrl);
            await ActivateHostedApplicationLayers(bpuaApplication, hostedApplicationLayersSection);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Activates hosted application layers
        /// </summary>
        /// <param name="bpuaApplication">BPUA application instance</param>
        /// <param name="hostedApplicationLayersSection">Configuration section for hosted application layers</param>
        public static async Task ActivateHostedApplicationLayers(IBPUAApplication bpuaApplication, IConfigurationSection hostedApplicationLayersSection)
        {
            foreach (HostedApplicationLayer hostedApplicationLayer in bpuaApplication.ServiceRegistry.EnumerateObjectsByType<HostedApplicationLayer>())
            {
                IBPUAIdentifier bpuaIdentifier = new BPUAIdentifier(hostedApplicationLayer.DomainName, hostedApplicationLayer.UseCaseName, hostedApplicationLayer.ApplicationLayerName, null, null);
                if (bpuaIdentifier.ApplicationLayerName == BPUA.Application.Contracts.ApplicationLayersNames.SL && hostedApplicationLayer.IsApplicationUseCaseLayer)
                {
                    await bpuaApplication.ExecuteTransition(bpuaIdentifier);
                }
            }
        }

        /// <summary>
        /// Creates BPUA identifier from configuration section.
        /// </summary>
        /// <param name="startupTransitionSection">Startup transition configuration section</param>
        /// <returns>Created BPUA identifier</returns>
        static IBPUAIdentifier CreateIdentifier(IConfigurationSection startupTransitionSection)
        {
            BPUAIdentifier bpuaIdentifier = new BPUAIdentifier();

            bpuaIdentifier.DomainName = ConfigurationReader.GetRequiredValue(startupTransitionSection, "DomainName");
            bpuaIdentifier.UseCaseName = ConfigurationReader.GetRequiredValue(startupTransitionSection, "UseCaseName");
            bpuaIdentifier.ApplicationLayerName = ConfigurationReader.GetRequiredValue(startupTransitionSection, "ApplicationLayerName");
            bpuaIdentifier.StateName = ConfigurationReader.GetOptionalValue(startupTransitionSection, "StateName");

            return bpuaIdentifier;
        }

        /// <summary>
        /// Registers hosted application layers
        /// </summary>
        /// <param name="bpuaApplication">BPUA application instance</param>
        /// <param name="hostedApplicationLayersSection">Configuration section for hosted application layers</param>
        /// <param name="hostUrl">Host URL</param>
        static void RegisterHostedApplicationLayers(IBPUAApplication bpuaApplication, IConfigurationSection hostedApplicationLayersSection, string hostUrl)
        {
            foreach (IConfigurationSection configuredHostedApplicationLayer in hostedApplicationLayersSection.GetChildren())
            {
                IBPUAIdentifier bpuaIdentifier = CreateIdentifier(configuredHostedApplicationLayer);
                string hostedApplicationLayerKey = KeyCompiler.CompileHostedApplicationLayerKey(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName);
                HostedApplicationLayer hostedApplicationLayer = new HostedApplicationLayer();
                hostedApplicationLayer.ApplicationLayerName = bpuaIdentifier.ApplicationLayerName;
                hostedApplicationLayer.DomainName = bpuaIdentifier.DomainName;
                hostedApplicationLayer.UseCaseName = bpuaIdentifier.UseCaseName;
                hostedApplicationLayer.HostUrl = hostUrl;

                string? isApplicationUseCaseLayer = ConfigurationReader.GetOptionalValue(configuredHostedApplicationLayer, "IsApplicationUseCaseLayer");
                if (!string.IsNullOrWhiteSpace(isApplicationUseCaseLayer) && bool.TryParse(isApplicationUseCaseLayer, out bool isApplicationUseCaseLayerValue))
                {
                    hostedApplicationLayer.IsApplicationUseCaseLayer = isApplicationUseCaseLayerValue;
                }

                if (!bpuaApplication.ServiceRegistry.TryRegisterObject(hostedApplicationLayerKey, hostedApplicationLayer))
                {
                    throw new InvalidOperationException($"Hosted application layer with key '{hostedApplicationLayerKey}' is already registered.");
                }
            }
        }
        #endregion
    }
}
