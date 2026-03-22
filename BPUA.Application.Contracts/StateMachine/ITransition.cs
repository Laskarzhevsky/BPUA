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
        /// Gets application layer name
        /// </summary>
        public string ApplicationLayerName
        {
            get;
        }

        /// <summary>
        /// Gets allowed caller type full names
        /// </summary>
        IReadOnlyList<string> AllowedCallerTypeFullNames
        {
            get;
        }

        /// <summary>
        /// Gets domain name
        /// </summary>
        string DomainName
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
        /// Gets name
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets source state name
        /// </summary>
        string SourceStateName
        {
            get;
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

        /// <summary>
        /// Gets use case name
        /// </summary>
        string UseCaseName
        {
            get;
        }
        #endregion
    }
}
