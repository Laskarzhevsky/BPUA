using BPUA.Application.Contracts;
using BPUA.Core;

using Microsoft.Extensions.Configuration;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.IData;

using System;
using System.Text;
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
            foreach (IConfigurationSection startupTransitionSection in hostedApplicationLayersSection.GetChildren())
            {
                IBPUAIdentifier bpuaIdentifier = CreateIdentifier(startupTransitionSection);
                string hostedApplicationLayerKey = KeyCompiler.CompileHostedApplicationLayerKey(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName);
                HostedApplicationLayer hostedApplicationLayer = new HostedApplicationLayer();
                hostedApplicationLayer.BPUAIdentifier = bpuaIdentifier;

                if (bpuaIdentifier.ApplicationLayerName == BPUA.Application.Contracts.ApplicationLayersNames.SL)
                {
                    UseCaseActivationResult useCaseActivationResult = await bpuaApplication.ActivateUseCaseAsync(bpuaIdentifier);
                    if (useCaseActivationResult.Succeeded)
                    {
                        string bpuaServicekey = KeyCompiler.CompileStateHandlerKey(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName, bpuaIdentifier.StateName);
                        IBPUAService? bpuaService = bpuaApplication.GetRequestHandler(bpuaServicekey);
                        if (bpuaService != null && bpuaService is IStateHandler)
                        {
                            IStateHandler stateHandler = (IStateHandler)bpuaService;
                            await stateHandler.Initialize();
                            if (stateHandler.BpuaIdentifier.StateName == BPUA.Application.Contracts.StateNames.INITIAL)
                            {
                                hostedApplicationLayer.HostedApplicationLayerState = HostedApplicationLayerState.Initialized;
                            }
                            else
                            {
                                hostedApplicationLayer.HostedApplicationLayerState = HostedApplicationLayerState.InitializationError;
                            }
                        }
                    }
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
                hostedApplicationLayer.BPUAIdentifier = bpuaIdentifier;

                if (bpuaApplication.ServiceRegistry.TryRegisterObject(hostedApplicationLayerKey, hostedApplicationLayer))
                {
                    if (bpuaIdentifier.ApplicationLayerName == BPUA.Application.Contracts.ApplicationLayersNames.SL)
                    {
                        continue;
                    }

                    hostedApplicationLayer.HostedApplicationLayerState = HostedApplicationLayerState.Initialized;
                }
                else
                {
                    hostedApplicationLayer.HostedApplicationLayerState = HostedApplicationLayerState.InitializationError;
                }
            }
        }
        #endregion
    }
}
