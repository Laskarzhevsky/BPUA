namespace BPUA.Application.NonFunctionalContracts
{
    /// <summary>
    /// Defines hosted application layer functionality
    /// </summary>
    public interface IHostedApplicationLayer
    {
        #region Properties
        /// <summary>
        /// Gets or sets application layer name
        /// </summary>
        string ApplicationLayerName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets domain name
        /// </summary>
        string DomainName
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
        string UseCaseName
        {
            get;
            set;
        }
        #endregion
    }
}