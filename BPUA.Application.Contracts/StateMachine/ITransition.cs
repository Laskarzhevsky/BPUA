using BPUA.Core;

using PocoDataSet.IData;

using System.Collections.Generic;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines transition functionality
    /// </summary>
    public interface ITransition
    {
        #region Properties
        /// <summary>
        /// Gets allowed caller type full names
        /// </summary>
        IReadOnlyList<string> AllowedCallerTypeFullNames
        {
            get;
        }

        /// <summary>
        /// Gets BPU identifier
        /// </summary>
        IBpuIdentifier BpuIdentifier
        {
            get;
        }

        /// <summary>
        /// Gets component identifier
        /// </summary>
        string ComponentIdentifier
        {
            get;
        }

        /// <summary>
        /// Gets flag indicating whether the transition is an endpoint in the use case.
        /// It can be called from outside of the use case.
        /// </summary>
        bool IsEndpoint
        {
            get;
        }

        /// <summary>
        /// Gets or sets request data context validation rules
        /// </summary>
        DistinctList<IValidationRule> RequestDataContextValidationRules
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets response data context validation rules
        /// </summary>
        DistinctList<IValidationRule> ResponseDataContextValidationRules
        {
            get; set;
        }

        /// <summary>
        /// Gets target state names
        /// </summary>
        IReadOnlyList<string> TargetStateNames
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Processes the request transition context
        /// </summary>
        /// <param name="requestTransitionContext">Request transition context</param>
        void ProcessRequestTransitionContext(IDataSet requestTransitionContext);

        /// <summary>
        /// Processes the response transition context
        /// </summary>
        /// <param name="responseTransitionContext">Response transition context</param>
        void ProcessResponseTransitionContext(IDataSet responseTransitionContext);
        #endregion
    }
}
