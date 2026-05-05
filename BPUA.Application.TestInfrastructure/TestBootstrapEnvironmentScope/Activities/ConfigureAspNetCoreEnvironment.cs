using System;
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
        /// <summary>
        /// Configures the ASP.NET Core environment for the scope by setting the <c>ASPNETCORE_ENVIRONMENT</c> variable
        /// and writing the environment-specific configuration file if provided.
        /// </summary>
        void ConfigureAspNetCoreEnvironment()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", AspNetCoreEnvironementName);
            if (!string.IsNullOrWhiteSpace(AspNetCoreEnvironementName))
            {
                if (!string.IsNullOrWhiteSpace(EnvironmentSpecificJson))
                {
                    string fileName = "appsettings." + AspNetCoreEnvironementName + ".json";
                    File.WriteAllText(Path.Combine(RootPath, fileName), EnvironmentSpecificJson, Encoding.UTF8);
                }
            }
        }
    }
}
