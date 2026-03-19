using BPUA.Application.Contracts;

namespace BPUA.Application.EventArguments
{
    /// <summary>
    /// Provides route transition context event arguments functionality
    /// </summary>
    public class RouteTransitionContextEventArgs : BPUAApplicationEventArgs
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="transitionContext">transition context</param>
        public RouteTransitionContextEventArgs(ITransitionContext? transitionContext)
        {
            TransitionContext = transitionContext;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets transition context
        /// </summary>
        public ITransitionContext? TransitionContext
        {
            get; set;
        }
        #endregion
    }
}
