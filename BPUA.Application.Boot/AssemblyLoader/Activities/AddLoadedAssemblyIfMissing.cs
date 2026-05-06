using System;
using System.Reflection;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides dynamic assemblies loader functionality
    /// </summary>
    public partial class DynamicAssembliesLoader
    {
        #region Methods
        /// <summary>
        /// Adds the currently loaded assembly to the list of loaded assemblies when it is not already present.
        /// </summary>
        void AddLoadedAssemblyIfMissing()
        {
            if (LoadedAssembly == null)
            {
                return;
            }

            for (int i = 0; i < ListOfLoadedAssemblies.Count; i++)
            {
                Assembly assembly = ListOfLoadedAssemblies[i];
                if (string.Equals(assembly.FullName, LoadedAssembly.FullName, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            ListOfLoadedAssemblies.Add(LoadedAssembly);
        }
        #endregion
    }
}
