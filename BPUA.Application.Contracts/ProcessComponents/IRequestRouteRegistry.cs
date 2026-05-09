namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines request route registry functionality.
    /// </summary>
    public interface IRequestRouteRegistry
    {
        #region Methods
        /// <summary>
        /// Gets transition
        /// </summary>
        /// <param name="requestorTypeFullName">Requestor type full name</param>
        /// <param name="eventName">Event name</param>
        /// <param name="transitionName">Transition name</param>
        /// <returns></returns>
        IRequestRoute? GetTransition(string requestorTypeFullName, string eventName, string? transitionName);

        /// <summary>
        /// Registers request route
        /// </summary>
        /// <param name="requestRoute">Request route</param>
        void RegisterRequestRoute(IRequestRoute requestRoute);
        #endregion
    }
}
