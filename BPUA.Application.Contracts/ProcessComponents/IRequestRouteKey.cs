namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines request route key functionality.
    /// The key identifies a request route definition that can be selected by the router.
    /// </summary>
    public interface IRequestRouteKey
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
