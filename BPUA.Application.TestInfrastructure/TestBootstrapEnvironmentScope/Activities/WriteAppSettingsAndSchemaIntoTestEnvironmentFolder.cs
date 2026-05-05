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
        /// Writes the appsettings and schema files into the temporary test environment folder.
        /// </summary>
        void WriteAppSettingsAndSchemaIntoTestEnvironmentFolder()
        {
            File.WriteAllText(Path.Combine(RootPath, "appsettings.json"), AppSettingsJson, Encoding.UTF8);
            WriteAppSettingsSchemaFile(AppSettingsJson, AppSettingsSchemaJson);
        }
        #endregion
    }
}
