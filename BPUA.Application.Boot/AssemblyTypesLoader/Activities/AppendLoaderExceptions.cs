using System;
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
