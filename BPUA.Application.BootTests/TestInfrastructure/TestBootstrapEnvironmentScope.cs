using System.IO;
using System.Text;

namespace BPUA.Application.BootTests.TestInfrastructure
{
    /// <summary>
    /// Creates an isolated temporary bootstrap environment for a test case.
    /// The scope writes configuration files, adjusts the current directory,
    /// and manages environment variables so the bootstrapper reads from a
    /// deterministic sandbox instead of the real machine or solution state.
    /// </summary>
    internal sealed class TestBootstrapEnvironmentScope : IDisposable
    {
        readonly string _originalCurrentDirectory;
        readonly string? _originalAspNetCoreEnvironment;
        readonly Dictionary<string, string?> _originalEnvironmentVariables = new Dictionary<string, string?>();

        /// <summary>
        /// Creates the temporary test environment and writes the requested configuration files.
        /// Optionally sets <c>ASPNETCORE_ENVIRONMENT</c> and writes the matching environment-specific
        /// configuration file so tests can exercise configuration precedence rules.
        /// </summary>
        /// <param name="appSettingsJson">Content of the base <c>appsettings.json</c> file.</param>
        /// <param name="environmentName">Optional ASP.NET Core environment name for the scope.</param>
        /// <param name="environmentSpecificJson">Optional content for <c>appsettings.{Environment}.json</c>.</param>
        public TestBootstrapEnvironmentScope(string appSettingsJson, string? environmentName = null, string? environmentSpecificJson = null)
        {
            _originalCurrentDirectory = Directory.GetCurrentDirectory();
            _originalAspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            RootPath = Path.Combine(Path.GetTempPath(), "BPUA.Application.BootTests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(RootPath);

            File.WriteAllText(Path.Combine(RootPath, "appsettings.json"), appSettingsJson, Encoding.UTF8);

            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environmentName);
                if (environmentSpecificJson != null)
                {
                    string fileName = "appsettings." + environmentName + ".json";
                    File.WriteAllText(Path.Combine(RootPath, fileName), environmentSpecificJson, Encoding.UTF8);
                }
            }
            else
            {
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", null);
            }

            Directory.SetCurrentDirectory(RootPath);
        }

        /// <summary>
        /// Gets the root directory of the temporary bootstrap sandbox.
        /// Tests use this path both for supplying the content root to the bootstrapper
        /// and for creating plugin folders and other auxiliary files.
        /// </summary>
        public string RootPath
        {
            get;
        }

        /// <summary>
        /// Creates a subdirectory under the temporary root and returns its absolute path.
        /// This is used to prepare plugin folders or simulated deployment locations expected by a test.
        /// </summary>
        /// <param name="relativePath">Relative path under the sandbox root.</param>
        /// <returns>Absolute path to the created directory.</returns>
        public string CreateDirectory(string relativePath)
        {
            string fullPath = Path.Combine(RootPath, relativePath);
            Directory.CreateDirectory(fullPath);
            return fullPath;
        }

        /// <summary>
        /// Sets an environment variable for the lifetime of the scope and records its previous value
        /// so it can be restored during disposal. This allows tests to verify configuration values that
        /// originate from environment variables without polluting the surrounding process.
        /// </summary>
        /// <param name="variableName">Name of the environment variable to set.</param>
        /// <param name="value">Scoped value to apply.</param>
        public void SetEnvironmentVariable(string variableName, string? value)
        {
            if (!_originalEnvironmentVariables.ContainsKey(variableName))
            {
                _originalEnvironmentVariables[variableName] = Environment.GetEnvironmentVariable(variableName);
            }

            Environment.SetEnvironmentVariable(variableName, value);
        }

        /// <summary>
        /// Restores the original process state and removes the temporary directory.
        /// Cleanup is best-effort for file deletion, but environment variables and the current
        /// directory are always restored so later tests run against a clean baseline.
        /// </summary>
        public void Dispose()
        {
            Directory.SetCurrentDirectory(_originalCurrentDirectory);
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", _originalAspNetCoreEnvironment);

            foreach (KeyValuePair<string, string?> item in _originalEnvironmentVariables)
            {
                Environment.SetEnvironmentVariable(item.Key, item.Value);
            }

            try
            {
                if (Directory.Exists(RootPath))
                {
                    Directory.Delete(RootPath, true);
                }
            }
            catch
            {
                // Best-effort cleanup only.
            }
        }
    }
}
