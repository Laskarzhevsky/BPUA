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
        /// Gets or sets BPUA identifier
        /// </summary>
        public IBPUAIdentifier BpuaIdentifier
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets the current initialization state of the hosted application layer
        /// </summary>
        public HostedApplicationLayerState HostedApplicationLayerState
        {
            get; set;
        } = HostedApplicationLayerState.NotInitialized;

        /// <summary>
        /// Gets or sets flag indicating whether it is the "Application" use case layer
        /// </summary>
        public bool IsApplicationUseCaseLayer
        {
            get; set;
        }
        #endregion
    }
}