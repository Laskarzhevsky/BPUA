using BPUA.Application.NonFunctionalContracts;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Represents hosted application layer registration
    /// </summary>
    public class HostedApplicationLayer : IHostedApplicationLayer
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets application layer name
        /// IHostedApplicationLayer interface implementation
        /// </summary>
        public string ApplicationLayerName
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets domain name
        /// IHostedApplicationLayer interface implementation
        /// </summary>
        public string DomainName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets flag indicating whether it is the "Application" use case layer
        /// IHostedApplicationLayer interface implementation
        /// </summary>
        public bool IsApplicationUseCaseLayer
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets use case name
        /// IHostedApplicationLayer interface implementation
        /// </summary>
        public string UseCaseName
        {
            get;
            set;
        } = default!;
        #endregion
    }
}