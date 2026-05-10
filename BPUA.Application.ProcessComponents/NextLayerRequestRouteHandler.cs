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
        /// <param name="requestHandlerKey">Request handler key</param>
        public NextLayerRequestRouteHandler(string requestHandlerKey) : base(requestHandlerKey)
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
