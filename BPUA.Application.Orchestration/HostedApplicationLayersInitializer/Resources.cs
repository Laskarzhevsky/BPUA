using Microsoft.Extensions.Configuration;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Provides hosted application layers initializer functionality
    /// </summary>
    public static partial class HostedApplicationLayersInitializer
    {
        #region Private Properties
        static IConfigurationSection HostedApplicationLayersSection
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets host URL
        /// </summary>
        static string HostUrl
        {
            get; set;
        } = default!;
        #endregion
    }
}
