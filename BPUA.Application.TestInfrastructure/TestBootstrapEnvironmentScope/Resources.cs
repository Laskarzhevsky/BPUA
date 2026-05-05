using System.Collections.Generic;

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
        #region Data Fields
        /// <summary>
        /// Gets or sets application settings JSON
        /// </summary>
        string AppSettingsJson
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets application settings schema JSON
        /// </summary>
        string? AppSettingsSchemaJson
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the ASP.NET Core environment name to set for the scope, e.g. "Development" or "Production".
        /// </summary>
        string? AspNetCoreEnvironementName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets environment-specific application settings JSON content for the scope,
        /// which will be written to <c>appsettings.{Environment}.json</c>
        /// if both this and <c>AspNetCoreEnvironementName</c> are provided.
        /// </summary>
        string? EnvironmentSpecificJson
        {
            get; set;
        }

        string OriginalCurrentDirectory
        {
            get; set;
        } = default!;

        string? OriginalAspNetCoreEnvironment
        {
            get; set;
        }

        Dictionary<string, string?> OriginalEnvironmentVariables
        {
            get; set;
        } = new Dictionary<string, string?>();
        #endregion
    }
}
