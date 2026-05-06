using System;
using System.Reflection;

using BPUA.Application.Contracts;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides dynamic assemblies loader functionality
    /// </summary>
    public partial class DynamicAssembliesLoader
    {
        #region Methods
        /// <summary>
        /// Loads assembly processors
        /// </summary>
        void LoadAssemblyProcessors()
        {
            Type?[] types;
            try
            {
                types = LoadedAssembly!.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                // Partial load — use the successfully loaded types
                types = ex.Types ?? Array.Empty<Type>();
            }

            for (int i = 0; i < types.Length; i++)
            {
                Type? type = types[i];
                if (type == null)
                {
                    continue;
                }

                if (!type.IsClass || type.IsAbstract)
                {
                    continue;
                }

                if (!typeof(IBpuaAssemblyProcessor).IsAssignableFrom(type))
                {
                    continue;
                }

                IBpuaAssemblyProcessor? bpuaAssemblyProcessor = Activator.CreateInstance(type) as IBpuaAssemblyProcessor;
                if (bpuaAssemblyProcessor != null)
                {
                    ListOfAssemblyProcessors.Add(bpuaAssemblyProcessor);
                    Console.WriteLine($"[AssemblyLoader] Loaded assembly processor: {type.FullName}");
                }
            }
        }
        #endregion
    }
}
