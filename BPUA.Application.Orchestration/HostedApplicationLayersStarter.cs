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
            IConfigurationSection HostedApplicationLayersSection = bpuaApplication.ApplicationConfiguration.GetSection("HostedApplicationLayers");
            if (!HostedApplicationLayersSection.Exists())
            {
                return;
            }

            foreach (IConfigurationSection startupTransitionSection in HostedApplicationLayersSection.GetChildren())
            {
                IBPUAIdentifier bpuaIdentifier = CreateIdentifier(startupTransitionSection);
                string hostedApplicationLayerKey = KeyCompiler.CompileHostedApplicationLayerKey(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName);
                HostedApplicationLayer hostedApplicationLayer = new HostedApplicationLayer();
                hostedApplicationLayer.BPUAIdentifier = bpuaIdentifier;

                if (bpuaIdentifier.ApplicationLayerName == BPUA.Application.Contracts.ApplicationLayersNames.SL)
                {
                    if (bpuaApplication.ServiceRegistry.TryRegisterObject(hostedApplicationLayerKey, hostedApplicationLayer))
                    {
                        hostedApplicationLayer.HostedApplicationLayerState = HostedApplicationLayerState.Initialized;
                    }
                    else
                    {
                        hostedApplicationLayer.HostedApplicationLayerState = HostedApplicationLayerState.InitializationError;
                        return;
                    }

                    UseCaseActivationResult useCaseActivationResult = await bpuaApplication.ActivateUseCaseAsync(bpuaIdentifier);
                    if (useCaseActivationResult.Succeeded)
                    {
                        string bpuaServicekey = KeyCompiler.CompileStateHandlerKey(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName, bpuaIdentifier.StateName);
                        IBPUAService? bpuaService = bpuaApplication.GetRequestHandler(bpuaServicekey);
                        if (bpuaService != null && bpuaService is IStateHandler)
                        {
                            IStateHandler stateHandler = (IStateHandler)bpuaService;
                            IDataSet? dataSet = await stateHandler.Initialize();
                            IRequestMetadata? requestMetadata = dataSet.GetRequestMetadata();
                            if (requestMetadata == null)
                            {
                                throw new InvalidOperationException("Request metadata is missing in the data set returned by state handler initialization. " + DescribeIdentifier(bpuaIdentifier));
                            }

                            if (requestMetadata.StateName == BPUA.Application.Contracts.StateNames.INITIAL)
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
                else
                {
                    if (bpuaApplication.ServiceRegistry.TryRegisterObject(hostedApplicationLayerKey, hostedApplicationLayer))
                    {
                        hostedApplicationLayer.HostedApplicationLayerState = HostedApplicationLayerState.Initialized;
                    }
                    else
                    {
                        hostedApplicationLayer.HostedApplicationLayerState = HostedApplicationLayerState.InitializationError;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Resolves transition handler for the specified startup transition identifier.
        /// </summary>
        /// <param name="bpuaIdentifier">BPUA identifier</param>
        /// <returns>Resolved transition handler</returns>
        public static ITransitionHandler ResolveTransitionHandler(IBPUAIdentifier bpuaIdentifier)
        {
            if (bpuaIdentifier == null)
            {
                throw new ArgumentNullException(nameof(bpuaIdentifier));
            }

            string transitionHandlerKey = KeyCompiler.CompileTransitionHandlerKey(
                bpuaIdentifier.DomainName,
                bpuaIdentifier.UseCaseName,
                bpuaIdentifier.ApplicationLayerName,
                bpuaIdentifier.StateName,
                bpuaIdentifier.TransitionName);

            ITransitionHandler? transitionHandler = BPUAApplication.GetInstance().GetRequestHandler(transitionHandlerKey) as ITransitionHandler;
            if (transitionHandler == null)
            {
                throw new InvalidOperationException(
                    "Transition handler was not found for application startup transition. " +
                    "TransitionHandlerKey='" + transitionHandlerKey + "'. " +
                    DescribeIdentifier(bpuaIdentifier));
            }

            return transitionHandler;
        }
        #endregion

        #region Private Methods
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
        /// Builds readable identifier description.
        /// </summary>
        /// <param name="bpuaIdentifier">BPUA identifier</param>
        /// <returns>Readable identifier description</returns>
        static string DescribeIdentifier(IBPUAIdentifier bpuaIdentifier)
        {
            return
                "DomainName='" + bpuaIdentifier.DomainName + "', " +
                "UseCaseName='" + bpuaIdentifier.UseCaseName + "', " +
                "ApplicationLayerName='" + bpuaIdentifier.ApplicationLayerName + "', " +
                "StateName='" + bpuaIdentifier.StateName + "', " +
                "TransitionName='" + bpuaIdentifier.TransitionName + "'";
        }

        /// <summary>
        /// Builds activation errors suffix.
        /// </summary>
        /// <param name="useCaseActivationResult">Use case activation result</param>
        /// <returns>Formatted activation errors suffix</returns>
        static string BuildErrorsSuffix(UseCaseActivationResult useCaseActivationResult)
        {
            if (useCaseActivationResult.Errors == null || useCaseActivationResult.Errors.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" Errors: ");

            for (int i = 0; i < useCaseActivationResult.Errors.Count; i++)
            {
                if (i > 0)
                {
                    stringBuilder.Append(" | ");
                }

                stringBuilder.Append(useCaseActivationResult.Errors[i]);
            }

            return stringBuilder.ToString();
        }
        #endregion
    }
}
