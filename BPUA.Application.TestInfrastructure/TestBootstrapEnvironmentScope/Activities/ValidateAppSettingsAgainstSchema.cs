using Json.Schema;

using System;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

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
        #region Private methods
        /// <summary>
        /// Validates the appsettings.json file against the provided JSON schema.
        /// </summary>
        void ValidateAppSettingsAgainstSchema()
        {
            JsonSchema schema = JsonSchema.FromText(AppSettingsSchemaJson!);
            using (JsonDocument jsonDocument = JsonDocument.Parse(AppSettingsJson))
            {
                EvaluationOptions evaluationOptions = new EvaluationOptions();
                evaluationOptions.OutputFormat = OutputFormat.List;
                EvaluationResults results = schema.Evaluate(jsonDocument.RootElement, evaluationOptions);
                if (!results.IsValid)
                {
                    StringBuilder errorMessageStringBuilder = new StringBuilder();
                    errorMessageStringBuilder.AppendLine("appsettings.json does not conform to the schema:");
                    AppendEvaluationErrors(errorMessageStringBuilder, results);
                    string compiledErrorMessage = errorMessageStringBuilder.ToString();
                    throw new InvalidOperationException(compiledErrorMessage);
                }
            }
        }

        /// <summary>
        /// Appends the evaluation errors from the JSON schema validation to the provided StringBuilder.
        /// </summary>
        /// <param name="errorMessageStringBuilder">The StringBuilder to append error messages to.</param>
        /// <param name="evaluationResults">The evaluation results containing errors.</param>
        void AppendEvaluationErrors(StringBuilder errorMessageStringBuilder, EvaluationResults evaluationResults)
        {
            if (evaluationResults.Errors != null)
            {
                foreach (var error in evaluationResults.Errors)
                {
                    string location = evaluationResults.InstanceLocation.ToString();
                    if (string.IsNullOrWhiteSpace(location))
                    {
                        location = "Root";
                    }

                    location = location.TrimStart('/');
                    location = location.Replace("/", ".");
                    location = Regex.Replace(location, @"\.(\d+)", "[$1]");

                    if (string.Equals(error.Value, "All values fail against the false schema", StringComparison.Ordinal))
                    {
                        if (!string.Equals(location, "/$schema", StringComparison.Ordinal))
                        {
                            errorMessageStringBuilder.AppendLine(location + ": Property is not allowed by the schema.");
                        }
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
        #endregion
    }
}
