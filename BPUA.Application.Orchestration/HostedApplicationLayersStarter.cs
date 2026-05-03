using BPUA.Application.Contracts;
using BPUA.Core;

using Microsoft.Extensions.Configuration;

using System;
using System.Threading.Tasks;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Provides functionality to prepare application startup transitions from configuration.
    /// </summary>
    public static class HostedApplicationLayersStarter
    {
        #region Public Methods
        /// <summary>
        /// Reads configured application startup transitions, creates BPUA identifiers for them,
        /// activates corresponding use cases, and returns created identifiers.
        /// </summary>
        /// <param name="configuration">Application configuration</param>
        /// <param name="bpuaApplication">BPUA application instance</param>
        public static async Task CreateAndActivateAsync(IBPUAApplication bpuaApplication)
        {
            IConfigurationSection hostedApplicationLayersSection = bpuaApplication.ApplicationConfiguration.GetSection("HostedApplicationLayers");
            if (!hostedApplicationLayersSection.Exists())
            {
                return;
            }

            RegisterHostedApplicationLayers(bpuaApplication, hostedApplicationLayersSection);
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

            bpuaIdentifier.DomainName = GetRequiredValue(startupTransitionSection, "DomainName");
            bpuaIdentifier.UseCaseName = GetRequiredValue(startupTransitionSection, "UseCaseName");
            bpuaIdentifier.ApplicationLayerName = GetRequiredValue(startupTransitionSection, "ApplicationLayerName");
            bpuaIdentifier.StateName = GetOptionalValue(startupTransitionSection, "StateName");

            return bpuaIdentifier;
        }

        /// <summary>
        /// Gets optional configuration value.
        /// </summary>
        /// <param name="section">Configuration section</param>
        /// <param name="key">Configuration key</param>
        /// <returns>Configuration value or empty string</returns>
        static string GetOptionalValue(IConfigurationSection section, string key)
        {
            string? value = section[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return value;
        }

        /// <summary>
        /// Gets required configuration value.
        /// </summary>
        /// <param name="section">Configuration section</param>
        /// <param name="key">Configuration key</param>
        /// <returns>Configuration value</returns>
        static string GetRequiredValue(IConfigurationSection section, string key)
        {
            string? value = section[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException("Required configuration value '" + key + "' is missing in HostedApplicationLayers.");
            }

            return value;
        }

        /// <summary>
        /// Registers hosted application layers
        /// </summary>
        /// <param name="bpuaApplication">BPUA application instance</param>
        /// <param name="hostedApplicationLayersSection">Configuration section for hosted application layers</param>
        static void RegisterHostedApplicationLayers(IBPUAApplication bpuaApplication, IConfigurationSection hostedApplicationLayersSection)
        {
            foreach (IConfigurationSection startupTransitionSection in hostedApplicationLayersSection.GetChildren())
            {
                IBPUAIdentifier bpuaIdentifier = CreateIdentifier(startupTransitionSection);
                string hostedApplicationLayerKey = KeyCompiler.CompileHostedApplicationLayerKey(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName);
                HostedApplicationLayer hostedApplicationLayer = new HostedApplicationLayer();
                hostedApplicationLayer.ApplicationLayerName = bpuaIdentifier.ApplicationLayerName;
                hostedApplicationLayer.DomainName = bpuaIdentifier.DomainName;
                hostedApplicationLayer.UseCaseName = bpuaIdentifier.UseCaseName;

                string? isApplicationUseCaseLayer = GetOptionalValue(startupTransitionSection, "IsApplicationUseCaseLayer");
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
