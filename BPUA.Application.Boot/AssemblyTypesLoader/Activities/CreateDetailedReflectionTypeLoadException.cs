using System;
using System.Reflection;
using System.Text;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides assembly types loader functionality.
    /// </summary>
    internal static partial class AssemblyTypesLoader
    {
        #region Private Methods
        /// <summary>
        /// Creates a detailed exception that preserves all loader diagnostics from
        /// <see cref="ReflectionTypeLoadException"/>.
        /// </summary>
        /// <param name="loadedAssembly">Assembly whose types were being loaded.</param>
        /// <param name="reflectionTypeLoadException">Original reflection exception.</param>
        /// <returns>
        /// An <see cref="InvalidOperationException"/> containing detailed loader information.
        /// </returns>
        static InvalidOperationException CreateDetailedReflectionTypeLoadException(Assembly loadedAssembly, ReflectionTypeLoadException reflectionTypeLoadException)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Failed to load one or more types from assembly.");
            stringBuilder.AppendLine("Assembly: " + loadedAssembly.FullName);
            stringBuilder.AppendLine();

            if (reflectionTypeLoadException.LoaderExceptions == null ||
                reflectionTypeLoadException.LoaderExceptions.Length == 0)
            {
                stringBuilder.AppendLine("No loader exceptions were provided by the runtime.");
            }
            else
            {
                AppendLoaderExceptions(stringBuilder, reflectionTypeLoadException.LoaderExceptions);
            }

            return new InvalidOperationException(stringBuilder.ToString(), reflectionTypeLoadException);
        }
        #endregion
    }
}
