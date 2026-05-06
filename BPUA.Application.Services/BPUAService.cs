using System;
using System.Threading.Tasks;

using BPUA.Application.Contracts;
using BPUA.Core;

namespace BPUA.Application.Services
{
    /// <summary>
    /// Provides BPUA service functionality
    /// </summary>
    /// <typeparam name="TArgs">Type of event arguments</typeparam>
    public abstract class BpuaService<TArgs> : AsyncDisposableObject, IBpuaService where TArgs : EventArgs
    {
        #region Public Methods
        /// <summary>
        /// Initializes component
        /// IBpuaService interface implementation
        /// </summary>
        /// <param name="bppApplication">BPUA application</param>
        public abstract Task InitializeComponent(IBpuaApplication bppApplication);
        #endregion

        #region Event handlers
        /// <summary>
        /// Handles event asynchronously
        /// </summary>
        /// <param name="sender">Event source</param>
        /// <param name="args">Strongly-typed event arguments</param>
        public abstract Task HandleAsync(object? sender, EventArgs args);

        /// <summary>
        /// Handles event asynchronously
        /// IBpuaService interface implementation
        /// </summary>
        /// <param name="sender">Event source</param>
        /// <param name="args">Event arguments</param>
        async Task IBpuaService.HandleAsync(object? sender, EventArgs args)
        {
            ServiceRequestEventArgs? serviceRequestEventArgs = args as ServiceRequestEventArgs;
            if (serviceRequestEventArgs == null)
            {
                return;
            }

            TArgs? typedEventArgs = serviceRequestEventArgs.EventArguments as TArgs;
            if (typedEventArgs == null)
            {
                return;
            }

            BpuaApplication = sender as IBpuaApplication;
            await HandleAsync(sender, args);
        }
        #endregion

        #region Protected Properties
        /// <summary>
        /// Gets or sets BPUA application
        /// </summary>
        protected IBpuaApplication? BpuaApplication
        {
            get; set;
        }
        #endregion
    }
}
