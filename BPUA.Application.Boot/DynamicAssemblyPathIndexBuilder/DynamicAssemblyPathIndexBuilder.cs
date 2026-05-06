using BPUA.Application.Contracts;

using System;
using System.IO;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides dynamic assembly path index builder functionality
    /// </summary>
    public static class DynamicAssemblyPathIndexBuilder
    {
        #region Public Methods
        /// <summary>
        /// Builds assembly path index
        /// </summary>
        /// <param name="pluginFolder">Plugin folder</param>
        /// <exception cref="DirectoryNotFoundException">Thrown if plugin folder not found</exception>
        /// <exception cref="InvalidOperationException">Thrown if plugin folde contains several assemblies with the same file name</exception>
        public static void BuildAssemblyPathIndex(string pluginFolder, IServiceRegistry serviceRegistry)
        {
            if (!Directory.Exists(pluginFolder))
            {
                throw new DirectoryNotFoundException("Plugin folder was not found: " + pluginFolder);
            }

            string[] files = Directory.GetFiles(pluginFolder, "*.dll", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                string fullPath = files[i];
                string assemblyName = Path.GetFileNameWithoutExtension(fullPath);
                string relativePath = Path.GetRelativePath(pluginFolder, fullPath);
                if (!serviceRegistry.TryRegisterDynamicAssemblyPath(assemblyName, relativePath))
                {
                    throw new InvalidOperationException($"Duplicate assembly name found during plugin scan: {assemblyName}. Existing path: {serviceRegistry.GetDynamicAssemblyPath(assemblyName)}. New path: {relativePath}.");
                }
            }
        }
        #endregion
    }
}
