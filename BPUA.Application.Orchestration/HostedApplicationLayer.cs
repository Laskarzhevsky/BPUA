using BPUA.Core;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Represents hosted application layer registration
    /// </summary>
    internal class HostedApplicationLayer
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets application layer name
        /// IBPUAIdentifier interface implementation
        /// </summary>
        public string? ApplicationLayerName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets domain name
        /// IBPUAIdentifier interface implementation
        /// </summary>
        public string? DomainName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets flag indicating whether it is the "Application" use case layer
        /// </summary>
        public bool IsApplicationUseCaseLayer
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets application layer name
        /// IBPUAIdentifier interface implementation
        /// </summary>
        public string? UseCaseName
        {
            get;
            set;
        }
        #endregion
    }
}