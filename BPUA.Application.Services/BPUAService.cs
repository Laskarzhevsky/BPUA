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
    public abstract class BPUAService<TArgs> : AsyncDisposableObject, IBPUAService where TArgs : EventArgs
    {
        #region Public Methods
        /// <summary>
        /// Initializes component
        /// IBPUAService interface implementation
        /// </summary>
        /// <param name="bppApplication">BPUA application</param>
        public abstract Task InitializeComponent(IBPUAApplication bppApplication);
        #endregion

        #region Event handlers
        /// <summary>
        /// Handles event asynchronously
        /// </summary>
        /// <param name="sender">Event source</param>
        /// <param name="args">Strongly-typed event arguments</param>
        public abstract Task HandleAsync(object? sender, TArgs args);

        /// <summary>
        /// Handles event asynchronously
        /// IBPUAService interface implementation
        /// </summary>
        /// <param name="sender">Event source</param>
        /// <param name="args">Event arguments</param>
        async Task IBPUAService.HandleAsync(object? sender, EventArgs args)
        {
            TArgs? typedEventArgs = args as TArgs;
            if (typedEventArgs == null)
            {
                return;
            }

            BPUAApplication = sender as IBPUAApplication;
            await HandleAsync(sender, typedEventArgs);
        }
        #endregion

        #region Protected Properties
        /// <summary>
        /// Gets or sets BPUA application
        /// </summary>
        protected IBPUAApplication? BPUAApplication
        {
            get; set;
        }
        #endregion
    }
}
