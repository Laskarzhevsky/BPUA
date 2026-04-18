using BPUA.Application.BusinessLogic;
using BPUA.Application.Contracts;
using BPUA.Core;

using PocoDataSet.BpuaExtensions;

using System.Collections.Generic;
using System.Reflection.Metadata;

namespace HR.Application.BL
{
    /// <summary>
    /// Provides functionality of the transition handler for "Initializing use case" transition of the account use case in the business logic application layer
    /// </summary>
    [RegisterAsBPUAService]
    public class InitializingApplicationTransitionHandler : BusinessLogicTransitionHandler, IBusinessLogicTransitionHandler
    {
        #region Identification
        public static string DomainName = HR.Application.Contracts.Contract.HR;
        public static string UseCaseName = HR.Application.Contracts.UseCaseNames.APPLICATION;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.BL;
        public static string StateName = HR.Application.Contracts.StateNames.WAITING_FOR_APPLICATION_LOAD;
        public static string TransitionName = HR.Application.Contracts.TransitionsNames.INITIALIZING_APPLICATION;

        /// <summary>
        /// Gets service keys
        /// </summary>
        public static string ServiceKey
        {
            get
            {
                return KeyCompiler.CompileTransitionHandlerKey(DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public InitializingApplicationTransitionHandler() : base(DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName)
        {
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Processes response
        /// </summary>
        protected override void ProcessResponse()
        {
            if (ResponseTransitionContext.HasError())
            {
                // TODO: send data context to BPUA Infrastructure Server for logging
                return;
            }
            else
            {
                IList<IMessage> messages = ResponseTransitionContext.GetMessages();
                int initializedApplicationLayersCount = 0;
                for (int i = 0; i < messages.Count; i++)
                {
                    IMessage message = messages[i];
                    if (message.MessageType == MessageType.Information && message.MessageText == BPUA.Application.Contracts.TextResources.ApplicationLayerInitialized)
                    {
                        initializedApplicationLayersCount++;
                    }
                }

                IRequestMetadata? requestMetadata = ResponseTransitionContext.GetRequestMetadata();
                if (requestMetadata == null)
                {
                    throw new System.ApplicationException("Request metadata is missing in data set.");
                }

                if (initializedApplicationLayersCount == 2)
                {
                    requestMetadata.StateName = BPUA.Application.Contracts.StateNames.INITIAL;
                }
                else
                {
                    // TODO: send data context to BPUA Infrastructure Server for logging
                }
            }
        }
        #endregion
    }
}
