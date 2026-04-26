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
        public SendRequestToApplicationNextLayerTransition(string requestName, string domainName, string useCaseName, string applicationLayerName, string stateName, string transitionName) : base(requestName, domainName, useCaseName, applicationLayerName, stateName, transitionName)
        {
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Processes the request transition context
        /// ITransition interface implementation
        /// </summary>
        /// <param name="requestTransitionContext">Request transition context</param>
        /// <param name="bpuaIdentifier">BPUA identifier</param>
        public override void ProcessRequestTransitionContext(IDataSet requestTransitionContext, IBPUAIdentifier bpuaIdentifier)
        {
            IBPUAIdentifier nextApplicationLayerBpuaIdentifier = bpuaIdentifier.Clone()!;
            switch (bpuaIdentifier.ApplicationLayerName)
            {
                case "SL":
                    nextApplicationLayerBpuaIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.BL;
                    break;
                case "BL":
                    nextApplicationLayerBpuaIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DPL;
                    break;
                case "DL":
                    nextApplicationLayerBpuaIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DAL;
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported application layer: {bpuaIdentifier.ApplicationLayerName}");
            }

            requestTransitionContext.AddRequestMetadata(nextApplicationLayerBpuaIdentifier);
        }

        /// <summary>
        /// Processes the response transition context
        /// </summary>
        /// <param name="responseTransitionContext">Response transition context</param>
        public override void ProcessResponseTransitionContext(IDataSet responseTransitionContext)
        {
            responseTransitionContext.RemoveLastRequestMetadata();
        }
        #endregion
    }
}
