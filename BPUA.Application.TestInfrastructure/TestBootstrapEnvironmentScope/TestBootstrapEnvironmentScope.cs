using System;
using System.Collections.Generic;
using System.IO;

namespace BPUA.Application.TestInfrastructure
{
    /// <summary>
    /// Creates an isolated temporary bootstrap environment for a test case.
    /// The scope writes configuration files, adjusts the current directory,
    /// and manages environment variables so the bootstrapper reads from a
    /// deterministic sandbox instead of the real machine or solution state.
    /// </summary>
    public partial class TestBootstrapEnvironmentScope : IDisposable
    {
        #region Constants
        /// <summary>
        /// Default appsettings schema file name.
        /// </summary>
        private const string DefaultAppSettingsSchemaFileName = "appsettings.schema.json";
        #endregion

        #region Constructors
        /// <summary>
        /// Creates the temporary test environment and writes the requested configuration files.
        /// Optionally sets <c>ASPNETCORE_ENVIRONMENT</c> and writes the matching environment-specific
        /// configuration file so tests can exercise configuration precedence rules.
        /// Optionally writes an appsettings JSON schema file beside <c>appsettings.json</c>.
        /// </summary>
        /// <param name="appSettingsJson">Content of the base <c>appsettings.json</c> file.</param>
        /// <param name="aspNetCoreEnvironementName">Optional ASP.NET Core environment name for the scope.</param>
        /// <param name="environmentSpecificJson">Optional content for <c>appsettings.{Environment}.json</c>.</param>
        /// <param name="appSettingsSchemaJson">Optional content of the appsettings JSON schema file.</param>
        public TestBootstrapEnvironmentScope(string appSettingsJson, string? aspNetCoreEnvironementName = null, string? environmentSpecificJson = null, string? appSettingsSchemaJson = null)
        {
            InitializeComponent(appSettingsJson, aspNetCoreEnvironementName, environmentSpecificJson, appSettingsSchemaJson);
            ValidateAppSettingsAgainstSchema();
            WriteAppSettingsAndSchemaIntoTestEnvironmentFolder();
            ConfigureAspNetCoreEnvironment();

            Directory.SetCurrentDirectory(RootPath);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the root directory of the temporary bootstrap sandbox.
        /// Tests use this path both for supplying the content root to the bootstrapper
        /// and for creating plugin folders and other auxiliary files.
        /// </summary>
        public string RootPath
        {
            get; private set;
        } = default!;
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates a subdirectory under the temporary root and returns its absolute path.
        /// This is used to prepare plugin folders or simulated deployment locations expected by a test.
        /// </summary>
        /// <param name="relativePath">Relative path under the sandbox root.</param>
        /// <returns>Absolute path to the created directory.</returns>
        public string CreateDirectory(string relativePath)
        {
            string fullPath = Path.Combine(RootPath, relativePath);
            Directory.CreateDirectory(fullPath);
            return fullPath;
        }

        /// <summary>
        /// Sets an environment variable for the lifetime of the scope and records its previous value
        /// so it can be restored during disposal. This allows tests to verify configuration values that
        /// originate from environment variables without polluting the surrounding process.
        /// </summary>
        /// <param name="variableName">Name of the environment variable to set.</param>
        /// <param name="value">Scoped value to apply.</param>
        public void SetEnvironmentVariable(string variableName, string? value)
        {
            if (!OriginalEnvironmentVariables.ContainsKey(variableName))
            {
                OriginalEnvironmentVariables[variableName] = Environment.GetEnvironmentVariable(variableName);
            }

            Environment.SetEnvironmentVariable(variableName, value);
        }
        #endregion

        #region Finalizers
        /// <summary>
        /// Restores the original process state and removes the temporary directory.
        /// Cleanup is best-effort for file deletion, but environment variables and the current
        /// directory are always restored so later tests run against a clean baseline.
        /// </summary>
        public void Dispose()
        {
            Directory.SetCurrentDirectory(OriginalCurrentDirectory);
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", OriginalAspNetCoreEnvironment);

            foreach (KeyValuePair<string, string?> item in OriginalEnvironmentVariables)
            {
                Environment.SetEnvironmentVariable(item.Key, item.Value);
            }

            try
            {
                if (Directory.Exists(RootPath))
                {
                    Directory.Delete(RootPath, true);
                }
            }
            catch
            {
                // Best-effort cleanup only.
            }
        }
        #endregion
    }
}
