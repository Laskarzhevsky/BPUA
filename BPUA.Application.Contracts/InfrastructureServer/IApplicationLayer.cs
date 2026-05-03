namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines hosted application layer contract.
    /// This contract is used to register hosted application layers with the infrastructure server.
    /// </summary>
    public interface IApplicationLayer
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
        /// Gets or set uniform resource locator
        /// </summary>
        string? Url
        {
            get; set;
        }
        #endregion
    }
}
