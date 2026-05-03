using BPUA.Core;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.IData;

using System;

namespace BPUA.Application.StateMachineComponents
{

    /// <summary>
    /// Provides transition definition functionality.
    /// </summary>
    public abstract class SendRequestToApplicationNextLayerTransition : Transition
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="requestName">Request name</param>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">Transition name</param>
        public SendRequestToApplicationNextLayerTransition(string requestName, IBPUAIdentifier bpuaIdentifier) : base(requestName, bpuaIdentifier)
        {
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Prepares the BPUA identifier for the next transition handler
        /// </summary>
        /// <param name= "nextTransitionHandlerBpuaIdentifier" >Next transition handler BPUA identifier</param>
        protected override void PrepareNextTransitionHandlerBpuaIdentifier(IBPUAIdentifier nextTransitionHandlerBpuaIdentifier)
        {
            switch (nextTransitionHandlerBpuaIdentifier.ApplicationLayerName)
            {
                case "SL":
                    nextTransitionHandlerBpuaIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.BL;
                    break;
                case "BL":
                    nextTransitionHandlerBpuaIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DPL;
                    break;
                case "DL":
                    nextTransitionHandlerBpuaIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DAL;
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported application layer: {nextTransitionHandlerBpuaIdentifier.ApplicationLayerName}");
            }
        }
        #endregion
    }
}
