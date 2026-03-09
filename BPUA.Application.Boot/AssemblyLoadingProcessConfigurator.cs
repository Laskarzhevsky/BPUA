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
        /// <returns>Application configuration</returns>
        public static IConfiguration LoadApplicationConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
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
        /// <param name="isDevelopmentEnvironment">Flag indicating whether application runs in development environment</param>
        /// <returns>Calculated path to folder with dynamic assemblies</returns>
        public static string CalculatePathToFolderWithDynamicAssemblies(IConfiguration applicationConfiguration, string pathToFolderWithExecutableFile, bool isDevelopmentEnvironment)
        {
            string? pluginPath = applicationConfiguration["PluginFolder"];
            if (string.IsNullOrEmpty(pluginPath))
            {
                throw new ArgumentOutOfRangeException(nameof(applicationConfiguration), "The appsettings.json file does not contain a 'PluginFolder' setting");
            }

            if (!isDevelopmentEnvironment)
            {
                pluginPath = Path.Combine(pathToFolderWithExecutableFile, pluginPath);
            }

            Console.WriteLine("PathToFolderWithDynamicAssemblies: " + pluginPath);
            return Path.GetFullPath(pluginPath);
        }
        #endregion
    }
}
