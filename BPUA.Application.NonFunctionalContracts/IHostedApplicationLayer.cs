namespace BPUA.Application.NonFunctionalContracts
{
    /// <summary>
    /// Defines hosted application layer functionality
    /// </summary>
    public interface IHostedApplicationLayer
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
        /// Gets or sets application layer name
        /// </summary>
        string? ApplicationLayerName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets domain name
        /// </summary>
        string? DomainName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets host URL for the hosted application layer.
        /// This is the URL on which the hosted application layer is accessible by remote hosts (callers).
        /// </summary>
        string? HostUrl
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets flag indicating whether it is the "Application" use case layer
        /// </summary>
        bool IsApplicationUseCaseLayer
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets use case name
        /// </summary>
        string? UseCaseName
        {
            get;
            set;
        }
        #endregion
    }
}