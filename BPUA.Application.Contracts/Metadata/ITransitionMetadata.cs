using BPUA.Core;

namespace BPUA.Application.Contracts
{
    public interface ITransitionMetadata : IBPUAIdentifier
    {
        #region Properties
        /// <summary>
        /// Gets or sets flag indicating whether transition is available
        /// </summary>
        bool Available
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets icon
        /// </summary>
        string? Icon
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets display text
        /// </summary>
        string? DisplayText
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets order
        /// </summary>
        int Order
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets flag indicating whether object is selected
        /// </summary>
        bool Selected
        {
            get; set;
        }
        #endregion
    }
}
