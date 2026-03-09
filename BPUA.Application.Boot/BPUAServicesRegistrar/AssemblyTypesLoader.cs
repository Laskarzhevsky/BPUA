using System;
using System.Reflection;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides assembly types loader functionality
    /// </summary>
    internal static class AssemblyTypesLoader
    {
        #region Public Methods
        /// <summary>
        /// Gets types from loaded assembly
        /// </summary>
        /// <param name="loadedAssembly">Loaded assembly</param>
        /// <returns>Types from loaded assembly</returns>
        public static Type[] GetTypesFromAssembly(Assembly loadedAssembly)
        {
            Type[] loadedTypes;
            try
            {
                loadedTypes = loadedAssembly.GetTypes();
            }
            catch (ReflectionTypeLoadException reflectionTypeLoadException)
            {
                // ex.Types can be null, and even when not null it may contain null slots.
                if (reflectionTypeLoadException.Types == null)
                {
                    loadedTypes = Array.Empty<Type>();
                }
                else
                {
                    // Copy only the non-null entries.
                    Type?[] typesFromReflectionTypeLoadException = reflectionTypeLoadException.Types;
                    int numberOfLoadedTypesFromReflectionTypeLoadException = CalculateNumberOfLoadedTypesFromReflectionTypeLoadException(typesFromReflectionTypeLoadException);
                    loadedTypes = CompilePartialLoadResult(numberOfLoadedTypesFromReflectionTypeLoadException, typesFromReflectionTypeLoadException);
                }
            }

            return loadedTypes;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Calculates number of loaded types from reflection type load exception
        /// </summary>
        /// <param name="typesFromReflectionTypeLoadException">Types from reflection type load exception</param>
        /// <returns>Number of loaded types from reflection type load exception</returns>
        static int CalculateNumberOfLoadedTypesFromReflectionTypeLoadException(Type?[] typesFromReflectionTypeLoadException)
        {
            int count = 0;
            for (int i = 0; i < typesFromReflectionTypeLoadException.Length; i++)
            {
                if (typesFromReflectionTypeLoadException[i] != null)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Compiles partial load result
        /// </summary>
        /// <param name="numberOfLoadedTypesFromReflectionTypeLoadException">Number of loaded types from reflection type load exception</param>
        /// <param name="typesFromReflectionTypeLoadException">Types from reflection type load exception</param>
        /// <returns>Compiled partial load result</returns>
        static Type[] CompilePartialLoadResult(int numberOfLoadedTypesFromReflectionTypeLoadException, Type?[] typesFromReflectionTypeLoadException)
        {
            Type[] partialLoadResult = new Type[numberOfLoadedTypesFromReflectionTypeLoadException];
            int j = 0;
            for (int i = 0; i < typesFromReflectionTypeLoadException.Length; i++)
            {
                if (typesFromReflectionTypeLoadException[i] != null)
                {
                    partialLoadResult[j++] = typesFromReflectionTypeLoadException[i]!;
                }
            }

            return partialLoadResult;
        }
        #endregion
    }
}
