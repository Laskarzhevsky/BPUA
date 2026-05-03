using System;
using System.Threading.Tasks;

using BPUA.Application.Contracts;
using BPUA.Application.Services;
using BPUA.Core;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.IData;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Provides request to the next layer event handler functionality
    /// </summary>
    [RegisterAsBPUAService]
    public class TransitionContextRouter : BPUAService<RouteTransitionContextEventArgs>
    {
        #region Public Methods
        /// <summary>
        /// Initializes component
        /// </summary>
        /// <param name="bppApplication">BPUA application</param>
        public override async Task InitializeComponent(IBPUAApplication bppApplication)
        {
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles event
        /// </summary>
        /// <param name="sender">Event source</param>
        /// <param name="args">Event arguments</param>
        public override async Task HandleAsync(object? sender, EventArgs args)
        {
            ServiceRequestEventArgs serviceRequestEventArgs = (ServiceRequestEventArgs)args;
            RouteTransitionContextEventArgs routeTransitionContextEventArgs = (RouteTransitionContextEventArgs)serviceRequestEventArgs.EventArguments;
            IDataSet? requestTransitionContext = routeTransitionContextEventArgs.TransitionContext;
            if (requestTransitionContext == null)
            {
                return;
            }

            IBPUAIdentifier? bpuaIdentifier = requestTransitionContext.GetCurrentBpuaIdentifier();
            if (bpuaIdentifier == null)
            {
                throw new System.Exception("BPUA identifier metadata is missing in data set.");
            }

            string hostedApplicationLayerKey = KeyCompiler.CompileHostedApplicationLayerKey(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName);
            object? hostedApplicationLayer;
            if (!BpuaApplication!.ServiceRegistry.TryGetRegisteredObject(hostedApplicationLayerKey, out hostedApplicationLayer))
            {
                return;
            }

//            bpuaIdentifier.RequestName = serviceRequestEventArgs.EventName;
            IDataSet? responseTransitionContext = null;
            UseCaseActivationResult useCaseActivationResult = await ((BPUAApplication)BpuaApplication).ActivateUseCaseAsync(bpuaIdentifier);
            if (useCaseActivationResult.Succeeded)
            {
                ITransition transition = GetTransition(requestTransitionContext, BpuaApplication.ServiceRegistry);
                transition.ProcessRequestTransitionContext(requestTransitionContext);
                if (requestTransitionContext.HasError())
                {
                    responseTransitionContext = requestTransitionContext;
                }
                else
                {
                    bpuaIdentifier = requestTransitionContext.GetCurrentBpuaIdentifier();
                    if (bpuaIdentifier == null)
                    {
                        throw new System.Exception("BPUA identifier metadata is missing in data set.");
                    }

                    useCaseActivationResult = await ((BPUAApplication)BpuaApplication).ActivateUseCaseAsync(bpuaIdentifier);
                    if (useCaseActivationResult.Succeeded)
                    {
                        string handlerTypeKey = KeyCompiler.CompileTransitionHandlerKey(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName, bpuaIdentifier.StateName, bpuaIdentifier.TransitionName);
                        ITransitionHandler? transitionHandler = BpuaApplication!.GetRequestHandler(handlerTypeKey) as ITransitionHandler;
                        if (transitionHandler != null)
                        {
                            transitionHandler.BPUAApplication = BpuaApplication;
                            await using (transitionHandler as IAsyncDisposable)
                            {
                                responseTransitionContext = await transitionHandler.HandleRequestAsync(requestTransitionContext);
                                if (responseTransitionContext == null)
                                {
                                    throw new System.Exception("Transition handler did not return a response transition context.");
                                }

                                transition.ProcessResponseTransitionContext(responseTransitionContext);
                            }
                        }
                    }
                    else
                    {
                        responseTransitionContext = requestTransitionContext;
                    }
                }
            }
            else
            {
                responseTransitionContext = requestTransitionContext;
            }

            BpuaApplication = null;
            routeTransitionContextEventArgs.TransitionContext = responseTransitionContext;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Prepares request transition context
        /// </summary>
        /// <param name="requestTransitionContext">Request transition context</param>
        /// <param name="serviceRegistry">Service registry</param>
        ITransition GetTransition(IDataSet requestTransitionContext, IServiceRegistry serviceRegistry)
        {
            IBPUAIdentifier? bpuaIdentifier = requestTransitionContext.GetCurrentBpuaIdentifier();
            if (bpuaIdentifier == null)
            {
                throw new System.Exception("BPUA identifier metadata is missing in data set.");
            }

            Type? transitionType = null;
            serviceRegistry.TryGetRegisteredTransitionType(bpuaIdentifier, out transitionType);
            if (transitionType == null)
            {
                throw new ApplicationException($"Transition is not registered for {bpuaIdentifier.RequestName}_{bpuaIdentifier.DomainName}_{bpuaIdentifier.UseCaseName}_{bpuaIdentifier.ApplicationLayerName}_{bpuaIdentifier.StateName}_{bpuaIdentifier.TransitionName}");
            }

            ITransition? transition = Activator.CreateInstance(transitionType) as ITransition;
            if (transition == null)
            {
                throw new ApplicationException($"Transition {bpuaIdentifier.RequestName}_{bpuaIdentifier.DomainName}_{bpuaIdentifier.UseCaseName}_{bpuaIdentifier.ApplicationLayerName}_{bpuaIdentifier.StateName}_{bpuaIdentifier.TransitionName} could not be instantiated.");
            }

            return transition;
        }
        #endregion
    }
}
