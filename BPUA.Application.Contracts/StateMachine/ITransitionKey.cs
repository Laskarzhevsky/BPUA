namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines transition selection key functionality.
    /// The key identifies a transition definition that can be selected by the router.
    /// </summary>
    public interface ITransitionKey
    {
        #region Properties
        /// <summary>
        /// Gets event name
        /// </summary>
        string EventName
        {
            get;
        }

        /// <summary>
        /// Gets the name of the requested transition, if any.
        /// </summary>
        /// <remarks>This property may return null if no transition has been requested.</remarks>
        string? RequestedTransitionName
        {
            get;
        }

        /// <summary>
        /// Gets requestor type full name
        /// </summary>
        string RequestorTypeFullName
        {
            get;
        }
        #endregion
    }
}
