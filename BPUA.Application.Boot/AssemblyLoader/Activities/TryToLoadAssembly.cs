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
        /// Tries to load assembly
        /// </summary>
        void TryToLoadAssembly()
        {
            try
            {
                LoadedAssembly = Assembly.LoadFrom(PathToDynamicAssembly);
            }
            catch (BadImageFormatException)
            {
                // Not a valid .NET assembly — ignore
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AssemblyLoader] Failed to load {PathToDynamicAssembly}: {ex.GetType().Name} - {ex.Message}");
            }
        }
        #endregion
    }
}
