using BPUA.Application.Contracts;
using BPUA.Application.ProcessComponents;
using BPUA.Core;

using PocoDataSet.BpuaExtensions;

namespace BPUA.InfrastructureServer.SL
{
    /// <summary>
    /// Provides functionality of the state handler for "WaitingForApplicationLoad" state of the InfrastructureServer use case in the state logic application layer
    /// </summary>
    [RegisterAsBpuaService]
    public class WaitingForApplicationLoadStateHandler : StateHandler
    {
        #region Identification
        public static string DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
        public static string UseCaseName = BPUA.InfrastructureServer.Contracts.UseCaseName.INFRASTRUCTURE_SERVER;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.SL;
        public static string StateName = BPUA.Application.Contracts.StateNames.WAITING_FOR_APPLICATION_LOAD;

        /// <summary>
        /// Gets service keys
        /// </summary>
        public static string ServiceKey
        {
            get
            {
                return KeyCompiler.CompileStateHandlerKey(DomainName, UseCaseName, ApplicationLayerName, StateName);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public WaitingForApplicationLoadStateHandler() : base(ServiceKey)
        {
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Processes response
        /// </summary>
        protected override void ProcessResponse()
        {
            StoreApplicationInitializationResultIntoBpuIdentifier();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Stores application initialization result into BPU identifier
        /// </summary>
        void StoreApplicationInitializationResultIntoBpuIdentifier()
        {
            IRequestMetadata? requestMetadata = ResponseTransitionContext.GetCurrentRequestMetadata();
            if (requestMetadata == null)
            {
                throw new System.ApplicationException("Request metadata is missing in data set.");
            }

            BpuIdentifier.StateName = requestMetadata.StateName;
        }
        #endregion
    }
}
