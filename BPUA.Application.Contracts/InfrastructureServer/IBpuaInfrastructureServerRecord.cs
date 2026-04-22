namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines BPUA infrastructure server record
    /// </summary>
    public interface IBpuaInfrastructureServerRecord
    {
        #region Properties
        /// <summary>
        /// Gets or sets application layer full name
        /// </summary>
        string? ApplicationLayerFullName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets flag indicating whehter DNS record needs to be registered with DNS server
        /// </summary>
        bool? RegisterWithDnsServer
        {
            get; set;
        }

        /// <summary>
        /// Gets or set uniform resource locator
        /// </summary>
        string? Url
        {
            get; set;
        }
        #endregion
    }
}
