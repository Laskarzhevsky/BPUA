using System;

namespace BPUA.Application.Contracts
{
    public sealed class ApplicationStateChangedEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">The name of the transition whose execution caused the state change</param>
        public ApplicationStateChangedEventArgs(string? domainName, string? useCaseName, string? applicationLayerName, string? stateName, string? transitionName)
        {
            DomainName = domainName;
            UseCaseName = useCaseName;
            ApplicationLayerName = applicationLayerName;
            StateName = stateName;
            TransitionName = transitionName;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets application layer name
        /// </summary>
        public string? ApplicationLayerName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets domain name
        /// </summary>
        public string? DomainName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets state name
        /// </summary>
        public string? StateName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets transition name
        /// </summary>
        public string? TransitionName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets application layer name
        /// </summary>
        public string? UseCaseName
        {
            get;
            set;
        }
        #endregion
    }
}
