using BPUA.Core;

using System;

namespace BPUA.Application.ProcessComponents
{

    /// <summary>
    /// Provides transition definition functionality.
    /// </summary>
    public abstract class NextLayerRequestRouteHandler : RequestRouteHandler
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="requestName">Request name</param>
        /// <param name="bpuIdentifier">BPU identifier</param>
        public NextLayerRequestRouteHandler(string requestName, IBpuIdentifier bpuIdentifier) : base(requestName, bpuIdentifier)
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="requestName">Request name</param>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        /// <param name="transitionName">Transition name</param>
        public NextLayerRequestRouteHandler(string requestName, string domainName, string useCaseName, string applicationLayerName, string stateName, string transitionName) : base(requestName, domainName, useCaseName, applicationLayerName, stateName, transitionName)
        {
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Prepares the BPU identifier for the next request handler
        /// </summary>
        /// <param name= "nextRequestHandlerBpuIdentifier" >Next request handler BPU identifier</param>
        protected override void PrepareNextRequestHandlerBpuIdentifier(IBpuIdentifier nextRequestHandlerBpuIdentifier)
        {
            switch (nextRequestHandlerBpuIdentifier.ApplicationLayerName)
            {
                case "SL":
                    nextRequestHandlerBpuIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.BL;
                    break;
                case "BL":
                    nextRequestHandlerBpuIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DPL;
                    break;
                case "DPL":
                    nextRequestHandlerBpuIdentifier.ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.DAL;
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported application layer: {nextRequestHandlerBpuIdentifier.ApplicationLayerName}");
            }
        }
        #endregion
    }
}
