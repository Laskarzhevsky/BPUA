using BPUA.Application.Contracts;

using System.Collections.Generic;

namespace BPUA.Application.Context
{
    /// <summary>
    /// Provides list of transition metadata functionality
    /// </summary>
    internal class ListOfTransitionMetadata : List<ITransitionMetadata>
    {
        #region Public Methods
        /// <summary>
        /// Adds transition metadata
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">Transition name</param>
        /// <param name="breadcrumbs">Breadcrumbs data</param>
        public void AddTransitionMetadata(string domainName, string useCaseName, string stateName, string transitionName, string? breadcrumbs = null)
        {
            ITransitionMetadata transitionMetadata = new TransitionMetadata();

            transitionMetadata.DomainName = domainName;
            transitionMetadata.UseCaseName = useCaseName;
            transitionMetadata.StateName = stateName;
            transitionMetadata.TransitionName = transitionName;
            transitionMetadata.Breadcrumbs = breadcrumbs;

            Add(transitionMetadata);
        }
        #endregion
    }
}
