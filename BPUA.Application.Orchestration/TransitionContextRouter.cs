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
        public override async Task HandleAsync(object? sender, RouteTransitionContextEventArgs args)
        {
            IDataSet? requestTransitionContext = args.TransitionContext;
            if (requestTransitionContext == null)
            {
                return;
            }

            IBPUAIdentifier? bpuaIdentifier = requestTransitionContext.GetBpuaIdentifier();
            if (bpuaIdentifier == null)
            {
                throw new System.Exception("BPUA identifier metadata is missing in data set.");
            }

            string handlerTypeKey = KeyCompiler.CompileTransitionHandlerKey(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName, bpuaIdentifier.StateName, bpuaIdentifier.TransitionName);

            IDataSet? responseTransitionContext = null;
            UseCaseActivationResult useCaseActivationResult = await BPUAApplication!.ActivateUseCaseAsync(bpuaIdentifier);
            if (useCaseActivationResult.Succeeded)
            {
                ITransitionHandler? transitionHandler = BPUAApplication!.GetRequestHandler(handlerTypeKey) as ITransitionHandler;
                if (transitionHandler != null)
                {
                    transitionHandler.BPUAApplication = BPUAApplication;
                    await using (transitionHandler as IAsyncDisposable)
                    {
                        responseTransitionContext = await transitionHandler.HandleRequestAsync(requestTransitionContext);
                    }
                }
            }
            else
            {
                responseTransitionContext = requestTransitionContext;
            }

            BPUAApplication = null;
            args.TransitionContext = responseTransitionContext;
        }
        #endregion
    }
}
