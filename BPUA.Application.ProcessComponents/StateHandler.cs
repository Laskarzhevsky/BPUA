using BPUA.Application.Contracts;
using BPUA.Core;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.Extensions;
using PocoDataSet.IData;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BPUA.Application.ProcessComponents
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
        List<IRequestRoute> _transitions = new List<IRequestRoute>();
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public StateHandler() : base()
        {
        }


        /// <summary>
        /// Creates an instance, taking the request handler key as arguments
        /// </summary>
        /// <param name="requestHandlerKey">Request handler key</param>
        public StateHandler(string requestHandlerKey) : base(requestHandlerKey)
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
                return KeyCompiler.CompileStateHandlerKey(BpuIdentifier.DomainName, BpuIdentifier.UseCaseName, BpuIdentifier.ApplicationLayerName, BpuIdentifier.StateName);
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
            dataSet.AddRequestMetadata(BpuIdentifier);
            await HandleRequestAsync(dataSet);

            if (ResponseTransitionContext.HasError())
            {
                return ResponseTransitionContext;
            }

            IRequestMetadata? requestMetadata = dataSet.GetCurrentRequestMetadata();
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
        protected void AddTransition(IRequestRoute transition)
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
