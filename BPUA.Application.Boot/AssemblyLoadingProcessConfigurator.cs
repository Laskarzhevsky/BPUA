using System;
using System.IO;

using Microsoft.Extensions.Configuration;

namespace BPUA.Application.Boot
{
    public static class AssemblyLoadingProcessConfigurator
    {
        #region Public Methods
        /// <summary>
        /// Loads application configuration
        /// </summary>
        /// <param name="pathToFolderWithExecutableFile">Path to folder with executable file</param>
        /// <returns>Application configuration</returns>
        public static IConfiguration LoadApplicationConfiguration(string pathToFolderWithExecutableFile)
        {
            AppSettingsSchemaValidator.ValidateAppSettingsAgainstSchema(pathToFolderWithExecutableFile, "appsettings.json");

            return new ConfigurationBuilder()
                .SetBasePath(pathToFolderWithExecutableFile)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Calculates path to folder with dynamic assemblies
        /// </summary>
        /// <param name="applicationConfiguration">Application configuration</param>
        /// <param name="pathToFolderWithExecutableFile">Path to folder with excutable file</param>
        /// <returns>Calculated path to folder with dynamic assemblies</returns>
        public static string CalculatePathToFolderWithDynamicAssemblies(IConfiguration applicationConfiguration, string pathToFolderWithExecutableFile)
        {
            string? pluginPath = applicationConfiguration["PluginFolder"];
            if (string.IsNullOrWhiteSpace(pluginPath))
            {
                throw new ArgumentOutOfRangeException(nameof(applicationConfiguration), "The appsettings.json file does not contain a 'PluginFolder' setting");
            }

            if (!Path.IsPathRooted(pluginPath))
            {
                pluginPath = Path.Combine(pathToFolderWithExecutableFile, pluginPath);
            }

            string pathToFolderWithDynamicAssemblies = Path.GetFullPath(pluginPath);
            return pathToFolderWithDynamicAssemblies;
        }
        #endregion
    }
}
