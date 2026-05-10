using System;
using System.Threading.Tasks;

using BPUA.Application.Contracts;
using BPUA.Application.Services;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.IData;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Provides request to the next layer event handler functionality
    /// </summary>
    [RegisterAsBpuaService]
    public partial class TransitionContextRouter : BpuaService<RouteTransitionContextEventArgs>
    {
        #region Public Methods
        /// <summary>
        /// Initializes component
        /// </summary>
        /// <param name="bppApplication">BPUA application</param>
        public override async Task InitializeComponent(IBpuaApplication bppApplication)
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
            RouteTransitionContextEventArgs routeTransitionContextEventArgs = (RouteTransitionContextEventArgs)args;
            IDataSet? requestTransitionContext = routeTransitionContextEventArgs.TransitionContext;
            if (requestTransitionContext == null)
            {
                return;
            }

            RequestTransitionContext = requestTransitionContext;
            await RouteTransitionContext();
            routeTransitionContextEventArgs.TransitionContext = ResponseTransitionContext;
        }
        #endregion
    }
}
