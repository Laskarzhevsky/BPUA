using Json.Schema;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Validates appsettings JSON files against their referenced JSON schema files.
    /// </summary>
    internal static class AppSettingsSchemaValidator
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

        #region Private Methods
        /// <summary>
        /// Appends validation errors to the error message string builder.
        /// </summary>
        /// <param name="errorMessageStringBuilder">Error message string builder.</param>
        /// <param name="evaluationResults">Evaluation results.</param>
        private static void AppendEvaluationErrors(StringBuilder errorMessageStringBuilder, EvaluationResults evaluationResults)
        {
            if (evaluationResults.Errors != null)
            {
                foreach (KeyValuePair<string, string> error in evaluationResults.Errors)
                {
                    string location = FormatInstanceLocation(evaluationResults.InstanceLocation.ToString());

                    if (string.Equals(error.Value, "All values fail against the false schema", StringComparison.Ordinal))
                    {
                        errorMessageStringBuilder.AppendLine(location + ": Property is not allowed by the schema.");
                    }
                    else
                    {
                        errorMessageStringBuilder.AppendLine(location + ": " + error.Value);
                    }
                }
            }

            if (evaluationResults.Details == null)
            {
                return;
            }

            foreach (EvaluationResults detail in evaluationResults.Details)
            {
                AppendEvaluationErrors(errorMessageStringBuilder, detail);
            }
        }

        /// <summary>
        /// Formats JSON schema instance location into a C# friendly location.
        /// </summary>
        /// <param name="instanceLocation">JSON schema instance location.</param>
        /// <returns>Formatted instance location.</returns>
        private static string FormatInstanceLocation(string instanceLocation)
        {
            string location = instanceLocation;
            if (string.IsNullOrWhiteSpace(location))
            {
                return "Root";
            }

            location = location.TrimStart('/');
            location = location.Replace("/", ".");
            location = Regex.Replace(location, @"\.(\d+)", "[$1]");

            if (string.IsNullOrWhiteSpace(location))
            {
                location = "Root";
            }

            return location;
        }

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

        /// <summary>
        /// Validates appsettings JSON against schema JSON.
        /// </summary>
        /// <param name="appSettingsFileName">Appsettings file name.</param>
        /// <param name="appSettingsJson">Appsettings JSON content.</param>
        /// <param name="appSettingsSchemaJson">Appsettings schema JSON content.</param>
        private static void ValidateAppSettingsJsonAgainstSchema(string appSettingsFileName, string appSettingsJson, string appSettingsSchemaJson)
        {
            JsonSchema schema = JsonSchema.FromText(appSettingsSchemaJson);

            using (JsonDocument jsonDocument = JsonDocument.Parse(appSettingsJson))
            {
                EvaluationOptions evaluationOptions = new EvaluationOptions();
                evaluationOptions.OutputFormat = OutputFormat.List;

                EvaluationResults results = schema.Evaluate(jsonDocument.RootElement, evaluationOptions);
                if (!results.IsValid)
                {
                    StringBuilder errorMessageStringBuilder = new StringBuilder();
                    errorMessageStringBuilder.AppendLine(appSettingsFileName + " does not conform to the schema:");
                    AppendEvaluationErrors(errorMessageStringBuilder, results);
                    string compiledErrorMessage = errorMessageStringBuilder.ToString();
                    throw new InvalidOperationException(compiledErrorMessage);
                }
            }
        }
        #endregion
    }
}
