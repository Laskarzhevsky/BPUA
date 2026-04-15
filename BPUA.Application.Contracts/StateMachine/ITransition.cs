using BPUA.Core;

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
        /// Gets BPUA identifier
        /// </summary>
        IBPUAIdentifier BpuaIdentifier
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
        /// Gets inbound data contract
        /// </summary>
        ITransitionDataContract InboundDataContract
        {
            get;
        }

        /// <summary>
        /// Gets  rosets flag indicating whether this is the default transition which starts from the specified state
        /// </summary>
        bool IsDefaultForState
        {
            get; set;
        }

        /// <summary>
        /// Gets outbound data contract
        /// </summary>
        ITransitionDataContract OutboundDataContract
        {
            get;
        }

        /// <summary>
        /// Gets target state names
        /// </summary>
        IReadOnlyList<string> TargetStateNames
        {
            get;
        }
        #endregion
    }
}
