using BPUA.Application.Contracts;

namespace BPUA.Application.RequestHandlers
{
    /// <summary>
    /// Provides transition handler functionality
    /// </summary>
    public abstract class DataAccessLogicTransitionHandler : TransitionHandler, IDataAccessLogicTransitionHandler
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public DataAccessLogicTransitionHandler() : base()
        {
        }

        /// <summary>
        /// Creates an instance, taking the transition handler identity as arguments
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">Transition name</param>
        public DataAccessLogicTransitionHandler(string domainName, string useCaseName, string applicationLayerName, string stateName, string transitionName) : base(domainName, useCaseName, applicationLayerName, stateName, transitionName)
        {
            TransitionNameAtRuntime = transitionName;
        }
        #endregion
    }
}
