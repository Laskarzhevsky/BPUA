using System;
using System.Collections.Generic;
using System.Reflection;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Event arguments for the "bootstrap completed" lifecycle event.
    /// </summary>
    /// <remarks>
    /// Raised at the end of the BPUA boot process, after all assemblies have been
    /// loaded, scanned, and registered, and all bootstrap events have been processed.
    /// </remarks>
    public sealed class BootstrapCompletedEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of <see cref="BootstrapCompletedEventArgs"/>.
        /// </summary>
        /// <param name="loadedAssemblies">The list of assemblies loaded during bootstrap.</param>
        /// <param name="serviceRegistry">The final service registry after bootstrap.</param>
        public BootstrapCompletedEventArgs(IReadOnlyList<Assembly> loadedAssemblies, IServiceRegistry serviceRegistry)
        {
            LoadedAssemblies = loadedAssemblies;
            ServiceRegistry = serviceRegistry;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the list of assemblies loaded during bootstrap.
        /// </summary>
        public IReadOnlyList<Assembly> LoadedAssemblies
        {
            get;
        }

        /// <summary>
        /// Gets the service registry containing all registered services.
        /// </summary>
        public IServiceRegistry ServiceRegistry
        {
            get;
        }
        #endregion
    }
}
