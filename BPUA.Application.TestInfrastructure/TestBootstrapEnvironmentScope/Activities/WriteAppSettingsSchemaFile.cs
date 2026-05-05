using System.IO;
using System.Text;

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
        /// Writes the appsettings schema file beside appsettings.json in the temporary bootstrap folder.
        /// </summary>
        /// <param name="appSettingsJson">Appsettings JSON content.</param>
        /// <param name="appSettingsSchemaJson">Optional schema JSON content supplied by the test.</param>
        private void WriteAppSettingsSchemaFile(string appSettingsJson, string? appSettingsSchemaJson)
        {
            string schemaFileName = ExtractAppSettingsSchemaFileName(appSettingsJson);
            string schemaFilePath = Path.Combine(RootPath, schemaFileName);

            if (!string.IsNullOrWhiteSpace(appSettingsSchemaJson))
            {
                File.WriteAllText(schemaFilePath, appSettingsSchemaJson, Encoding.UTF8);
            }
        }
        #endregion
    }
}
