using System;
using System.IO;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides dynamic assemblies loader functionality
    /// </summary>
    public partial class DynamicAssembliesLoader
    {
        #region Methods
        /// <summary>
        /// Loads dynamic assemblies
        /// </summary>
        void LoadDynamicAssemblies()
        {
            if (!Directory.Exists(PathToFolderWithDynamicAssemblies))
            {
                Console.WriteLine($"[AssemblyLoader] Folder not found: {PathToFolderWithDynamicAssemblies}");
                return;
            }

            string[] pathsToDynamicAssemblies = Directory.GetFiles(PathToFolderWithDynamicAssemblies, "*.dll");
            for (int i = 0; i < pathsToDynamicAssemblies.Length; i++)
            {
                LoadedAssembly = null;
                PathToDynamicAssembly = pathsToDynamicAssemblies[i];
                TryToLoadAssembly();
                if (LoadedAssembly == null)
                {
                    continue;
                }

                if (HasLoadBPUAAssemblyAttribute())
                {
                    Console.WriteLine($"[AssemblyLoader] Loaded assembly {PathToDynamicAssembly}");
                    if (HasProvideBPUAProcessorsAttribute())
                    {
                        LoadAssemblyProcessors();
                    }
                    else
                    {
                        AddLoadedAssemblyIfMissing();
                    }
                }
            }
        }
        #endregion
    }
}
