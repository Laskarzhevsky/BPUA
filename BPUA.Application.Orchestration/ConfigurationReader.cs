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
        public static string? GetOptionalValue(IBPUAApplication bpuaApplication, string key)
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
        public static string? GetOptionalValue(IConfigurationSection section, string key)
        {
            string? value;
            if (string.IsNullOrWhiteSpace(key))
            {
                value = section.Value;
            }
            else if (string.Equals(section.Key, key, StringComparison.Ordinal))
            {
                value = section.Value;
            }
            else
            {
                value = section[key];
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
        public static string GetRequiredValue(IConfigurationSection section, string? key = null)
        {
            string? value;
            if (string.IsNullOrWhiteSpace(key))
            {
                value = section.Value;
            }
            else if (string.Equals(section.Key, key, StringComparison.Ordinal))
            {
                value = section.Value;
            }
            else
            {
                value = section[key];
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException("Required configuration value is missing.");
            }

            return value;
        }
        #endregion
    }
}
