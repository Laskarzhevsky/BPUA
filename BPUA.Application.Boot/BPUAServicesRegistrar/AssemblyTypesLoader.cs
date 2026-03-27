using System;
using System.Reflection;
using System.Text;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides assembly types loader functionality.
    /// </summary>
    internal static class AssemblyTypesLoader
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
        static InvalidOperationException CreateDetailedReflectionTypeLoadException(
            Assembly loadedAssembly,
            ReflectionTypeLoadException reflectionTypeLoadException)
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

        /// <summary>
        /// Appends all loader exceptions to the supplied string builder.
        /// </summary>
        /// <param name="stringBuilder">String builder that collects the diagnostic text.</param>
        /// <param name="loaderExceptions">Loader exceptions reported by the runtime.</param>
        static void AppendLoaderExceptions(StringBuilder stringBuilder, Exception[] loaderExceptions)
        {
            for (int i = 0; i < loaderExceptions.Length; i++)
            {
                Exception loaderException = loaderExceptions[i];
                if (loaderException != null)
                {
                    stringBuilder.AppendLine("Loader exception " + i + ":");
                    stringBuilder.AppendLine(loaderException.ToString());
                    stringBuilder.AppendLine();
                }
            }
        }
        #endregion
    }
}
