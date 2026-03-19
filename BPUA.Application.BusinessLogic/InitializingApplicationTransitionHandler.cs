using BPUA.Application.Contracts;
using BPUA.Application.RequestHandlers;
using BPUA.Core;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPUA.Application.BusinessLogic
{
    [RegisterAsBPUAService]
    public class InitializingApplicationTransitionHandler : BusinessLogicTransitionHandler, IBusinessLogicTransitionHandler
    {
        #region Identification
        public static string DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
        public static string UseCaseName = BPUA.Application.Contracts.UseCaseNames.APPLICATION;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.BL;
        public static string StateName = default!;
        public static string TransitionName = BPUA.Application.Contracts.TransitionsNames.INITIALIZING_APPLICATION;

        /// <summary>
        /// Gets service key
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

        #region Public Methods
        /// <summary>
        /// Handles request
        /// </summary>
        /// <param name="requestTransitionContext">Request transition context</param>
        /// <returns>Response transition context</returns>
        public override async Task<ITransitionContext?> HandleRequestAsync(ITransitionContext? requestTransitionContext)
        {
            if (requestTransitionContext == null)
            {
                return requestTransitionContext;
            }

            RouteTransitionContextEventArgs routeTransitionContextEventArgs = new RouteTransitionContextEventArgs(requestTransitionContext);
            await RaiseServiceRequestEventAsync(routeTransitionContextEventArgs);

            ITransitionContext? responseTransitionContext = routeTransitionContextEventArgs.TransitionContext;
            if (responseTransitionContext == null)
            {
                return responseTransitionContext;
            }

            IReadOnlyList<ITransitionMetadata> transitionsMetadata = responseTransitionContext.TransitionsMetadata;
            for (int i = 0; i < transitionsMetadata.Count; i++)
            {
                ITransitionMetadata transitionMetadata = transitionsMetadata[i];
                transitionMetadata.Available = true;
            }

            return responseTransitionContext;
        }
        #endregion
    }
}
