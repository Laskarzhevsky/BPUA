using Json.Schema;

using System;
using System.Text;
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
