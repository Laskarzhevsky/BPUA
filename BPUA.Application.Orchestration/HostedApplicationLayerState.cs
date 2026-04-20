namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Defines hosted application layer state
    /// </summary>
    internal enum HostedApplicationLayerState
    {
        /// <summary>
        /// Initialization error
        /// </summary>
        InitializationError,
        
        /// <summary>
        /// Successfully initialized
        /// </summary>
        Initialized,
        
        /// <summary>
        /// Not initialized
        /// </summary>
        NotInitialized,
    }
}
