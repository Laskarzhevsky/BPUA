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
        public static async Task Initialize(IBpuaApplication bpuaApplication)
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
        public static async Task ActivateHostedApplicationLayers(IBpuaApplication bpuaApplication, IConfigurationSection hostedApplicationLayersSection)
        {
            foreach (HostedApplicationLayer hostedApplicationLayer in bpuaApplication.ServiceRegistry.EnumerateObjectsByType<HostedApplicationLayer>())
            {
                IBpuIdentifier bpuIdentifier = new BpuIdentifier(hostedApplicationLayer.DomainName, hostedApplicationLayer.UseCaseName, hostedApplicationLayer.ApplicationLayerName, null, null);
                if (bpuIdentifier.ApplicationLayerName == BPUA.Application.Contracts.ApplicationLayersNames.SL && hostedApplicationLayer.IsApplicationUseCaseLayer)
                {
                    await bpuaApplication.ExecuteTransition(bpuIdentifier);
                }
            }
        }

        /// <summary>
        /// Creates BPU identifier from configuration section.
        /// </summary>
        /// <param name="startupTransitionSection">Startup transition configuration section</param>
        /// <returns>Created BPU identifier</returns>
        static IBpuIdentifier CreateIdentifier(IConfigurationSection startupTransitionSection)
        {
            BpuIdentifier bpuIdentifier = new BpuIdentifier();

            bpuIdentifier.DomainName = ConfigurationReader.GetRequiredValue(startupTransitionSection, "DomainName");
            bpuIdentifier.UseCaseName = ConfigurationReader.GetRequiredValue(startupTransitionSection, "UseCaseName");
            bpuIdentifier.ApplicationLayerName = ConfigurationReader.GetRequiredValue(startupTransitionSection, "ApplicationLayerName");
            bpuIdentifier.StateName = ConfigurationReader.GetOptionalValue(startupTransitionSection, "StateName");

            return bpuIdentifier;
        }

        /// <summary>
        /// Registers hosted application layers
        /// </summary>
        /// <param name="bpuaApplication">BPUA application instance</param>
        /// <param name="hostedApplicationLayersSection">Configuration section for hosted application layers</param>
        /// <param name="hostUrl">Host URL</param>
        static void RegisterHostedApplicationLayers(IBpuaApplication bpuaApplication, IConfigurationSection hostedApplicationLayersSection, string hostUrl)
        {
            foreach (IConfigurationSection configuredHostedApplicationLayer in hostedApplicationLayersSection.GetChildren())
            {
                IBpuIdentifier bpuIdentifier = CreateIdentifier(configuredHostedApplicationLayer);
                string hostedApplicationLayerKey = KeyCompiler.CompileHostedApplicationLayerKey(bpuIdentifier.DomainName, bpuIdentifier.UseCaseName, bpuIdentifier.ApplicationLayerName);
                HostedApplicationLayer hostedApplicationLayer = new HostedApplicationLayer();
                hostedApplicationLayer.ApplicationLayerName = bpuIdentifier.ApplicationLayerName;
                hostedApplicationLayer.DomainName = bpuIdentifier.DomainName;
                hostedApplicationLayer.UseCaseName = bpuIdentifier.UseCaseName;
                hostedApplicationLayer.Url = hostUrl;

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
