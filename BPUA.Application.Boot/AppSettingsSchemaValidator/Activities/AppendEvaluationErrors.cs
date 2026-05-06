using Json.Schema;

using System;
using System.Collections.Generic;
using System.Text;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Validates appsettings JSON files against their referenced JSON schema files.
    /// </summary>
    internal static partial class AppSettingsSchemaValidator
    {
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
        #endregion
    }
}
