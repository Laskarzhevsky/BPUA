namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines transition registry functionality.
    /// </summary>
    public interface ITransitionRegistry
    {
        #region Methods
        /// <summary>
        /// Gets transition
        /// </summary>
        /// <param name="requestorTypeFullName">Requestor type full name</param>
        /// <param name="eventName">Event name</param>
        /// <param name="transitionName">Transition name</param>
        /// <returns></returns>
        ITransition? GetTransition(string requestorTypeFullName, string eventName, string? transitionName);

        /// <summary>
        /// Registers transition
        /// </summary>
        /// <param name="transitionForRegistration">Transition for registration</param>
        void RegisterTransition(ITransition transitionForRegistration);
        #endregion
    }
}
