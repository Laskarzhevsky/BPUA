using BPUA.Application.Contracts;
using BPUA.Core;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.Extensions;
using PocoDataSet.IData;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BPUA.Application.StateMachineComponents
{
    /// <summary>
    /// Provides state handler functionality
    /// </summary>
    public abstract class StateHandler : RequestHandler, IStateHandler
    {
        #region Data Fields
        /// <summary>
        /// Holds transitions of the state handler, where key is transition name and value is transition
        /// </summary>
        List<ITransition> _transitions = new List<ITransition>();
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public StateHandler() : base()
        {
        }

        /// <summary>
        /// Creates an instance, taking the state handler identity as arguments
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        public StateHandler(string domainName, string useCaseName, string applicationLayerName, string stateName) : base(domainName, useCaseName, applicationLayerName, stateName)
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
                return KeyCompiler.CompileStateHandlerKey(BpuaIdentifier.DomainName, BpuaIdentifier.UseCaseName, BpuaIdentifier.ApplicationLayerName, BpuaIdentifier.StateName);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initializes the state handler
        /// IStateHandler interface implementation
        /// </summary>
        /// <returns>Response transition context</returns>
        public async Task<IDataSet?> Initialize()
        {
            IDataSet dataSet = DataSetFactory.CreateDataSet();
            dataSet.AddRequestMetadata(BpuaIdentifier);
            await HandleRequestAsync(dataSet);

            if (ResponseTransitionContext.HasError())
            {
                return ResponseTransitionContext;
            }

            IRequestMetadata? requestMetadata = dataSet.GetRequestMetadata();
            if (requestMetadata == null)
            {
                return ResponseTransitionContext;
            }

            requestMetadata.StateName = BPUA.Application.Contracts.StateNames.INITIAL;

            return ResponseTransitionContext;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Adds transition
        /// </summary>
        /// <param name="transition">Transition for addition</param>
        protected void AddTransition(ITransition transition)
        {
            string transitionIdentifier = transition.ComponentIdentifier;
            if (_transitions.Exists(t => t.ComponentIdentifier == transitionIdentifier))
            {
                return;
            }

            _transitions.Add(transition);
        }
        #endregion
    }
}
