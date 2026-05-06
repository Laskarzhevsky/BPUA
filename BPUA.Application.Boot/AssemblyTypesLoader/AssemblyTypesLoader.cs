using System;
using System.Reflection;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides assembly types loader functionality.
    /// </summary>
    internal static partial class AssemblyTypesLoader
    {
        #region Public Methods
        /// <summary>
        /// Gets all types from the specified assembly.
        /// </summary>
        /// <param name="loadedAssembly">Assembly whose types must be loaded.</param>
        /// <returns>Array of types defined in the assembly.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="loadedAssembly"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the runtime cannot load one or more types from the assembly.
        /// The exception message contains the assembly name and all loader exceptions.
        /// </exception>
        public static Type[] GetTypesFromAssembly(Assembly loadedAssembly)
        {
            if (loadedAssembly == null)
            {
                throw new ArgumentNullException(nameof(loadedAssembly));
            }

            try
            {
                return loadedAssembly.GetTypes();
            }
            catch (ReflectionTypeLoadException reflectionTypeLoadException)
            {
                throw CreateDetailedReflectionTypeLoadException(loadedAssembly, reflectionTypeLoadException);
            }
        }
        #endregion
    }
}
