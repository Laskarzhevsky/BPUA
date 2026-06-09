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
                DynamicAssemblyDependencyResolver.RegisterDynamicAssemblyPath(PathToDynamicAssembly);
                LoadedAssembly = Assembly.LoadFrom(PathToDynamicAssembly);
            }
            catch (BadImageFormatException)
            {
                // Not a valid .NET assembly — ignore
                LoadedAssembly = null;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Failed to load dynamic assembly. Path: " + PathToDynamicAssembly, exception);
            }
        }
        #endregion
    }
}
