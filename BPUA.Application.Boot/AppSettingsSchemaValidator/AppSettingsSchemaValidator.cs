using System.IO;
using System.Text;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Validates appsettings JSON files against their referenced JSON schema files.
    /// </summary>
    internal static partial class AppSettingsSchemaValidator
    {
        #region Public Methods
        /// <summary>
        /// Validates the appsettings JSON file against the JSON schema referenced by its $schema property.
        /// If the appsettings file does not reference a schema, validation is skipped.
        /// </summary>
        /// <param name="configurationFolderPath">Configuration folder path.</param>
        /// <param name="appSettingsFileName">Appsettings file name.</param>
        public static void ValidateAppSettingsAgainstSchema(string configurationFolderPath, string appSettingsFileName)
        {
            string appSettingsFilePath = Path.Combine(configurationFolderPath, appSettingsFileName);
            if (!File.Exists(appSettingsFilePath))
            {
                return;
            }

            string appSettingsJson = File.ReadAllText(appSettingsFilePath, Encoding.UTF8);
            string? schemaFilePath = TryGetSchemaFilePath(configurationFolderPath, appSettingsJson);
            if (string.IsNullOrWhiteSpace(schemaFilePath))
            {
                return;
            }

            if (!File.Exists(schemaFilePath))
            {
                throw new FileNotFoundException("The appsettings schema file was not found.", schemaFilePath);
            }

            string appSettingsSchemaJson = File.ReadAllText(schemaFilePath, Encoding.UTF8);
            ValidateAppSettingsJsonAgainstSchema(appSettingsFileName, appSettingsJson, appSettingsSchemaJson);
        }
        #endregion
    }
}
