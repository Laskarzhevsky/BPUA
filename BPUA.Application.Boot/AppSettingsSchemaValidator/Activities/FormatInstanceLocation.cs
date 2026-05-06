using System.Text.RegularExpressions;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Validates appsettings JSON files against their referenced JSON schema files.
    /// </summary>
    internal static partial class AppSettingsSchemaValidator
    {
        #region Private Methods
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
        #endregion
    }
}
