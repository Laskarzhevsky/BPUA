using BPUA.Application.Contracts;
using BPUA.Core;

namespace BPUA.Application.StateMachineComponents
{
    /// <summary>
    /// Provides transition handler functionality
    /// </summary>
    public abstract class TransitionHandler : RequestHandler, ITransitionHandler
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public TransitionHandler() : base()
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
        public TransitionHandler(string domainName, string useCaseName, string applicationLayerName, string stateName, string transitionName) : base(domainName, useCaseName, applicationLayerName, stateName, transitionName)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets component identifier
        /// </summary>
        public override string ComponentIdentifier
        {
            get
            {
                return KeyCompiler.CompileTransitionHandlerKey(BpuaIdentifier.DomainName, BpuaIdentifier.UseCaseName, BpuaIdentifier.ApplicationLayerName, BpuaIdentifier.StateName, BpuaIdentifier.TransitionName);
            }
        }
        #endregion
    }
}
