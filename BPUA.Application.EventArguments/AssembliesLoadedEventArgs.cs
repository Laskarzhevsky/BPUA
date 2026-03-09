using System;
using System.Collections.Generic;
using System.Reflection;

namespace BPUA.Application.EventArguments
{
    /// <summary>
    /// Event arguments for the "assemblies loaded" bootstrap lifecycle event.
    /// </summary>
    /// <remarks>
    /// Raised during the BPUA boot process after all plugin assemblies have
    /// been successfully loaded, but before per-assembly processing begins.
    /// </remarks>
    public sealed class AssembliesLoadedEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of <see cref="AssembliesLoadedEventArgs"/>.
        /// </summary>
        /// <param name="loadedAssemblies">The list of loaded assemblies.</param>
        public AssembliesLoadedEventArgs(IReadOnlyList<Assembly> loadedAssemblies)
        {
            LoadedAssemblies = loadedAssemblies;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the list of loaded assemblies.
        /// </summary>
        public IReadOnlyList<Assembly> LoadedAssemblies
        {
            get;
        }
        #endregion
    }
}
