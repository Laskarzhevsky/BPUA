using System.IO;
using System.Text.Json;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Validates appsettings JSON files against their referenced JSON schema files.
    /// </summary>
    internal static partial class AppSettingsSchemaValidator
    {
        #region Private Methods
        /// <summary>
        /// Gets schema file path from appsettings JSON content when the $schema property exists.
        /// </summary>
        /// <param name="configurationFolderPath">Configuration folder path.</param>
        /// <param name="appSettingsJson">Appsettings JSON content.</param>
        /// <returns>Schema file path, or null when no schema reference exists.</returns>
        private static string? TryGetSchemaFilePath(string configurationFolderPath, string appSettingsJson)
        {
            using (JsonDocument jsonDocument = JsonDocument.Parse(appSettingsJson))
            {
                JsonElement rootElement = jsonDocument.RootElement;
                if (rootElement.ValueKind != JsonValueKind.Object)
                {
                    return null;
                }

                JsonElement schemaElement;
                if (!rootElement.TryGetProperty("$schema", out schemaElement))
                {
                    return null;
                }

                if (schemaElement.ValueKind != JsonValueKind.String)
                {
                    return null;
                }

                string? schemaPath = schemaElement.GetString();
                if (string.IsNullOrWhiteSpace(schemaPath))
                {
                    return null;
                }

                if (Path.IsPathRooted(schemaPath))
                {
                    return schemaPath;
                }

                return Path.GetFullPath(Path.Combine(configurationFolderPath, schemaPath));
            }
        }
        #endregion
    }
}
