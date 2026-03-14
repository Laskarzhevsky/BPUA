using BPUA.Application.Contracts;
using BPUA.Core;

namespace BPUA.Application.Context
{
    /// <summary>
    /// Provides transition metadata functionality
    /// </summary>
    public class TransitionMetadata : BPUAIdentifier, ITransitionMetadata
    {
        #region Properties
        /// <summary>
        /// Gets or sets flag indicating whether transition is available
        /// ITransitionMetadata interface implementation
        /// </summary>
        public bool Available
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets icon
        /// ITransitionMetadata interface implementation
        /// </summary>
        public string? Icon
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets display text
        /// ITransitionMetadata interface implementation
        /// </summary>
        public string? DisplayText
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets order
        /// ITransitionMetadata interface implementation
        /// </summary>
        public int Order
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets flag indicating whether object is selected
        /// ITransitionMetadata interface implementation
        /// </summary>
        public bool Selected
        {
            get; set;
        }
        #endregion
    }
}
