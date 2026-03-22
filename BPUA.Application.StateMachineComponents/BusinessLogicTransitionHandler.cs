using System.Collections.Generic;
using System.Threading.Tasks;

using BPUA.Application.Context;
using BPUA.Application.Contracts;
using BPUA.Core;

using PocoDataSet.IData;

namespace BPUA.Application.StateMachineComponents
{
    /// <summary>
    /// Provides transition handler functionality
    /// </summary>
    public abstract class BusinessLogicTransitionHandler : TransitionHandler, IBusinessLogicTransitionHandler
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public BusinessLogicTransitionHandler() : base()
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
        public BusinessLogicTransitionHandler(string domainName, string useCaseName, string applicationLayerName, string stateName, string transitionName) : base(domainName, useCaseName, applicationLayerName, stateName, transitionName)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Handles request
        /// </summary>
        /// <param name="requestTransitionContext">Request transition context</param>
        /// <returns>Response transition context</returns>
        public override async Task<ITransitionContext?> HandleRequestAsync(ITransitionContext? requestTransitionContext)
        {
            ITransitionContext? responseTransitionContext = await base.HandleRequestAsync(requestTransitionContext);
            if (responseTransitionContext == null)
            {
                return requestTransitionContext;
            }

            IRequestMetadata requestMetadata = responseTransitionContext.RequestMetadata;
            if (string.IsNullOrEmpty(requestMetadata.StateName))
            {
                requestMetadata.StateName = BPUA.Application.Contracts.StateNames.INITIAL;
            }
            
//            AddTransitionsMetadataToResponse();
            return responseTransitionContext;
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

            IServiceRegistry serviceRegistry = BPUAApplication.ServiceRegistry;
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
