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
        /// Extracts the schema file name referenced by the appsettings JSON content.
        /// </summary>
        /// <param name="appSettingsJson">Appsettings JSON content.</param>
        /// <returns>Schema file name.</returns>
        private string ExtractAppSettingsSchemaFileName(string appSettingsJson)
        {
            string schemaFileName = DefaultAppSettingsSchemaFileName;
            string schemaPropertyName = "\"$schema\"";
            int schemaPropertyIndex = appSettingsJson.IndexOf(schemaPropertyName, StringComparison.Ordinal);
            if (schemaPropertyIndex < 0)
            {
                return schemaFileName;
            }

            int colonIndex = appSettingsJson.IndexOf(':', schemaPropertyIndex + schemaPropertyName.Length);
            if (colonIndex < 0)
            {
                return schemaFileName;
            }

            int valueStartIndex = appSettingsJson.IndexOf('"', colonIndex + 1);
            if (valueStartIndex < 0)
            {
                return schemaFileName;
            }

            int valueEndIndex = appSettingsJson.IndexOf('"', valueStartIndex + 1);
            if (valueEndIndex < 0)
            {
                return schemaFileName;
            }

            string schemaPath = appSettingsJson.Substring(valueStartIndex + 1, valueEndIndex - valueStartIndex - 1);
            if (string.IsNullOrWhiteSpace(schemaPath))
            {
                return schemaFileName;
            }

            schemaFileName = Path.GetFileName(schemaPath);
            if (string.IsNullOrWhiteSpace(schemaFileName))
            {
                schemaFileName = DefaultAppSettingsSchemaFileName;
            }

            return schemaFileName;
        }
        #endregion
    }
}
