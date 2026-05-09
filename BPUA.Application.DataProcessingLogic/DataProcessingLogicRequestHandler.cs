using BPUA.Application.Contracts;
using BPUA.Application.ProcessComponents;

namespace BPUA.Application.DataProcessingLogic
{
    /// <summary>
    /// Provides transition handler functionality
    /// </summary>
    public abstract class DataProcessingLogicRequestHandler : RequestHandler, IDataAccessLogicTransitionHandler
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public DataProcessingLogicRequestHandler() : base()
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
        public DataProcessingLogicRequestHandler(string domainName, string useCaseName, string applicationLayerName, string stateName, string transitionName) : base(domainName, useCaseName, applicationLayerName, stateName, transitionName)
        {
        }
        #endregion
    }
}
