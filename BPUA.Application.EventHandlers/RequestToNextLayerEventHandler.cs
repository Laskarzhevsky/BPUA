using System;
using System.Threading.Tasks;

using BPUA.Application.Contracts;
using BPUA.Application.EventArguments;
using BPUA.Application.Orchestration;
using BPUA.Application.Services;
using BPUA.Core;

using PocoDataSet.IData;

namespace BPUA.Application.EventHandlers
{
    /// <summary>
    /// Provides request to the next layer event handler functionality
    /// </summary>
    [RegisterAsBPUAService]
    public class RequestToNextLayerEventHandler : BPUAService<RequestToNextLayerEventArgs>
    {
        #region Public Methods
        /// <summary>
        /// Initializes component
        /// </summary>
        /// <param name="bppApplication">BPUA application</param>
        public override void InitializeComponent(IBPUAApplication bppApplication)
        {
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles event
        /// </summary>
        /// <param name="sender">Event source</param>
        /// <param name="args">Event arguments</param>
        public override async Task HandleAsync(object? sender, RequestToNextLayerEventArgs args)
        {
            ITransitionContext? requestTransitionContext = args.TransitionContext;
            if (requestTransitionContext == null)
            {
                return;
            }

            IBPUAIdentifier bpuaIdentifier = requestTransitionContext.BPUAIdentifier;
            string? applicationNextLayerName = BPUAApplicationLayers.GetNextLayerName(bpuaIdentifier.ApplicationLayerName);
            requestTransitionContext.AddRequestMetadata(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, applicationNextLayerName, bpuaIdentifier.StateName, bpuaIdentifier.TransitionName, bpuaIdentifier.Breadcrumbs);
            string handlerTypeKey = KeyCompiler.CompileTransitionHandlerKey(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, applicationNextLayerName, bpuaIdentifier.StateName, bpuaIdentifier.TransitionName);

            ITransitionContext? responseTransitionContext = null;
            ITransitionHandler? transitionHandler = BPUAApplication!.GetRequestHandler(handlerTypeKey) as ITransitionHandler;
            if (transitionHandler != null)
            {
                transitionHandler.BPUAApplication = BPUAApplication;
                await using (transitionHandler as IAsyncDisposable)
                {
                    responseTransitionContext = await transitionHandler.HandleRequestAsync(requestTransitionContext);
                    if (responseTransitionContext != null)
                    {
                        responseTransitionContext.RemoveCurrentRequestMetadata();
                    }
                }
            }

            BPUAApplication = null;
            args.TransitionContext = responseTransitionContext;
        }
        #endregion
    }
}
