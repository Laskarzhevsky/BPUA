using BPUA.Application.Contracts;
using BPUA.Application.ProcessComponents;

namespace BPUA.Application.BusinessLogic
{
    /// <summary>
    /// Provides request handler functionality
    /// </summary>
    public abstract class BusinessLogicRequestHandler : RequestHandler, IBusinessLogicRequestHandler
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public BusinessLogicRequestHandler() : base()
        {
        }

        /// <summary>
        /// Creates an instance, taking the request handler key as arguments
        /// </summary>
        /// <param name="requestHandlerKey">Request handler key</param>
        public BusinessLogicRequestHandler(string requestHandlerKey) : base(requestHandlerKey)
        {
        }
        #endregion

        /*
                #region Private Methods
                /// <summary>
                /// Adds transitions metadata to response
                /// </summary>
                void AddTransitionsMetadataToResponse()
                {
                    IRequestMetadata requestMetadata = RequestTransitionContext!.RequestMetadata;
                    string stateHandlerKey = KeyCompiler.CompileStateHandlerKey(requestMetadata.DomainName, requestMetadata.UseCaseName, requestMetadata.ApplicationLayerName, requestMetadata.StateName);

                    IServiceRegistry serviceRegistry = BpuaApplication.ServiceRegistry;
                    IList<string>? listOfUseCaseStateTransitionNames = serviceRegistry.GetUseCaseStateTransitionNames(stateHandlerKey);
                    if (listOfUseCaseStateTransitionNames == null)
                    {
                        return;
                    }

                    if (ResponseTransitionContext == null)
                    {
                        return;
                    }

                    for (int i = 0; i < listOfUseCaseStateTransitionNames.Count; i++)
                    {
                        if (listOfUseCaseStateTransitionNames[i] == BPUA.Application.Contracts.TransitionsNames.INITIALIZING_USE_CASE)
                        {
                            continue;
                        }

                        ITransitionMetadata newTransitionMetadata = new TransitionMetadata();
                        newTransitionMetadata.DomainName = requestMetadata.DomainName;
                        newTransitionMetadata.UseCaseName = requestMetadata.UseCaseName;
                        newTransitionMetadata.StateName = requestMetadata.StateName;
                        newTransitionMetadata.TransitionName = listOfUseCaseStateTransitionNames[i];
                    }
                }
                #endregion
        */
    }
}
