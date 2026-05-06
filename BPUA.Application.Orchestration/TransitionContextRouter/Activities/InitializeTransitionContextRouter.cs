using BPUA.Core;

using PocoDataSet.BpuaExtensions;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Provides request to the next layer event handler functionality
    /// </summary>
    public partial class TransitionContextRouter
    {
        #region Private Methods
        /// <summary>
        /// Initializes the transition context router by extracting necessary information from the request transition context
        /// </summary>
        /// <param name="requestTransitionContext">The request transition context</param>
        void InitializeTransitionContextRouter()
        {
            ResponseTransitionContext = RequestTransitionContext;
            IBpuIdentifier? bpuIdentifier = RequestTransitionContext.GetCurrentBpuIdentifier();
            if (bpuIdentifier == null)
            {
                throw new System.Exception("BPU identifier metadata is missing in data set.");
            }

            BpuIdentifier = bpuIdentifier;
        }
        #endregion
    }
}
