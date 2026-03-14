using BPUA.Application.Contracts;

namespace BPUA.Application.EventArguments
{
    /// <summary>
    /// Provides request to the next layer event arguments functionality
    /// </summary>
    public class RequestToNextLayerEventArgs : BPUAApplicationEventArgs
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="transitionContext">transition context</param>
        public RequestToNextLayerEventArgs(ITransitionContext? transitionContext)
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
