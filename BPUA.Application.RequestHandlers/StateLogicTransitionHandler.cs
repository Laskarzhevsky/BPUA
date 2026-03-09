using System.Collections.Generic;
using System.Threading.Tasks;

using BPUA.Application.Contracts;
using PocoDataSet.BPUAExtensions;
using BPUA.Core;

using PocoDataSet.IData;

namespace BPUA.Application.RequestHandlers
{
    /// <summary>
    /// Provides transition handler functionality
    /// </summary>
    public abstract class StateLogicTransitionHandler : TransitionHandler, IStateLogicTransitionHandler
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public StateLogicTransitionHandler() : base()
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
        public StateLogicTransitionHandler(string domainName, string useCaseName, string applicationLayerName, string stateName, string transitionName) : base(domainName, useCaseName, applicationLayerName, stateName, transitionName)
        {
        }
        #endregion
    }
}
