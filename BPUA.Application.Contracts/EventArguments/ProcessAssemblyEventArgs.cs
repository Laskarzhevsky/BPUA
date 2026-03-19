using System;
using System.Reflection;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Event arguments for the "process assembly" bootstrap lifecycle event.
    /// </summary>
    /// <remarks>
    /// Raised once for each loaded assembly during the bootstrap sequence,
    /// allowing handlers to perform assembly-specific initialization or registration.
    /// </remarks>
    public sealed class ProcessAssemblyEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of <see cref="ProcessAssemblyEventArgs"/>.
        /// </summary>
        /// <param name="assembly">The assembly being processed.</param>
        /// <param name="serviceRegistry">The shared service registry for bootstrap.</param>
        public ProcessAssemblyEventArgs(Assembly assembly, IServiceRegistry serviceRegistry)
        {
            Assembly = assembly;
            ServiceRegistry = serviceRegistry;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the assembly being processed.
        /// </summary>
        public Assembly Assembly
        {
            get;
        }

        /// <summary>
        /// Gets the service registry used for service registration during bootstrap.
        /// </summary>
        public IServiceRegistry ServiceRegistry
        {
            get;
        }
        #endregion
    }
}
