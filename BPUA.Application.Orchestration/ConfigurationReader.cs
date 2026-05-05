using BPUA.Application.Contracts;

using Microsoft.Extensions.Configuration;

using System;

namespace BPUA.Application.Orchestration
{
    internal static class ConfigurationReader
    {
        #region Public Methods
        /// <summary>
        /// Gets optional configuration value.
        /// </summary>
        /// <param name="section">Configuration section</param>
        /// <param name="key">Configuration key</param>
        /// <returns>Configuration value or empty string</returns>
        public static string GetOptionalValue(IBPUAApplication bpuaApplication, string key)
        {
            IConfigurationSection section = bpuaApplication.ApplicationConfiguration.GetSection(key);
            string? value = GetOptionalValue(section, key);

            return value;
        }

        /// <summary>
        /// Gets optional configuration value.
        /// </summary>
        /// <param name="section">Configuration section</param>
        /// <param name="key">Configuration key</param>
        /// <returns>Configuration value or empty string</returns>
        public static string GetOptionalValue(IConfigurationSection section, string key)
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
        public static string GetRequiredValue(IBPUAApplication bpuaApplication, string key)
        {
            IConfigurationSection section = bpuaApplication.ApplicationConfiguration.GetSection(key);
            string value = GetRequiredValue(section, key);

            return value;
        }

        /// <summary>
        /// Gets required configuration value.
        /// </summary>
        /// <param name="section">Configuration section</param>
        /// <param name="key">Configuration key</param>
        /// <returns>Configuration value</returns>
        public static string GetRequiredValue(IConfigurationSection section, string key)
        {
            string? value = section[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException("Required configuration value '" + key + "' is missing in HostedApplicationLayers.");
            }

            return value;
        }
        #endregion
    }
}
