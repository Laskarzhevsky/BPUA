using BPUA.Application.Contracts;
using BPUA.Core;

using PocoDataSet.IData;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Provides request to the next layer event handler functionality
    /// </summary>
    public partial class TransitionContextRouter
    {
        #region Private Properties
        /// <summary>
        /// Gets or sets the BPU identifier
        /// </summary>
        IBpuIdentifier BpuIdentifier
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets request transition context
        /// </summary>
        IDataSet RequestTransitionContext
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets response transition context
        /// </summary>
        IDataSet? ResponseTransitionContext
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets transition
        /// </summary>
        ITransition Transition
        {
            get; set;
        } = default!;

        /// <summary>
        /// Gets or sets transition handler
        /// </summary>
        ITransitionHandler TransitionHandler
        {
            get; set;
        } = default!;
        #endregion

        #region Finalizers
        /// <summary>
        /// Releases resources
        /// </summary>
        protected override void ReleaseResources()
        {
            BpuIdentifier = default!;
            RequestTransitionContext = default!;
            ResponseTransitionContext = default!;
            Transition = default!;
            TransitionHandler = default!;
        }
        #endregion
    }
}
