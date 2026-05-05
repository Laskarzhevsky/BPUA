using System;
using System.IO;

namespace BPUA.Application.TestInfrastructure
{
    /// <summary>
    /// Creates an isolated temporary bootstrap environment for a test case.
    /// The scope writes configuration files, adjusts the current directory,
    /// and manages environment variables so the bootstrapper reads from a
    /// deterministic sandbox instead of the real machine or solution state.
    /// </summary>
    public partial class TestBootstrapEnvironmentScope
    {
        #region Private Methods
        /// <summary>
        /// Initializes somponent
        /// </summary>
        /// <param name="appSettingsJson">Content of the base <c>appsettings.json</c> file.</param>
        /// <param name="aspNetCoreEnvironementName">Optional ASP.NET Core environment name for the scope.</param>
        /// <param name="environmentSpecificJson">Optional content for <c>appsettings.{Environment}.json</c>.</param>
        /// <param name="appSettingsSchemaJson">Optional content of the appsettings JSON schema file.</param>
        void InitializeComponent(string appSettingsJson, string? aspNetCoreEnvironementName, string? environmentSpecificJson, string? appSettingsSchemaJson)
        {
            AppSettingsJson = appSettingsJson;
            AspNetCoreEnvironementName = aspNetCoreEnvironementName;
            EnvironmentSpecificJson = environmentSpecificJson;
            AppSettingsSchemaJson = appSettingsSchemaJson;
            OriginalCurrentDirectory = Directory.GetCurrentDirectory();
            OriginalAspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            RootPath = Path.Combine(Path.GetTempPath(), "BPUA.Application.BootTests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(RootPath);
        }
        #endregion
    }
}
