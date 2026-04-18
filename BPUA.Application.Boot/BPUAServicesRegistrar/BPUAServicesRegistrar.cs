using System;
using System.Reflection;

using BPUA.Application.Contracts;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides BPUA services registrar functionality
    /// </summary>
    public static class BPUAServicesRegistrar
    {
        #region Public Methods
        /// <summary>
        /// Registers services from assembly
        /// </summary>
        /// <param name="loadedAssembly">Loaded assembly</param>
        /// <param name="serviceRegistry">Service registry</param>
        public static void RegisterServicesFromAssembly(Assembly loadedAssembly, IServiceRegistry serviceRegistry)
        {
            Type[] types = AssemblyTypesLoader.GetTypesFromAssembly(loadedAssembly);
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

                if (!type.IsDefined(typeof(RegisterAsBPUAServiceAttribute), inherit: false))
                {
                    continue;
                }

                string? resolvedServiceKey = ServiceKeyResolver.TryToResolveServiceKey(type);
                if (string.IsNullOrEmpty(resolvedServiceKey))
                {
                    string[]? resolvedServiceKeys = ServiceKeyResolver.TryToResolveServiceKeys(type);
                    if (resolvedServiceKeys == null || resolvedServiceKeys.Length == 0)
                    {
                        string? resolvedEventArgsKey = ServiceKeyResolver.TryToResolveEventArgsKey(type);
                        if (string.IsNullOrEmpty(resolvedEventArgsKey))
                        {
                            string? key = type.FullName;
                            if (!string.IsNullOrEmpty(key))
                            {
                                serviceRegistry.RegisterType(key, type);
                                Console.WriteLine($"[RegisterServicesFromAssembly] Registered service {key}: {type.FullName}");
                            }
                        }
                        else
                        {
                            serviceRegistry.RegisterType(resolvedEventArgsKey, type);
                            Console.WriteLine($"[RegisterServicesFromAssembly] Registered service {resolvedEventArgsKey}: {type.FullName}");
                        }
                    }
                    else
                    {
                        for (int j = 0; j < resolvedServiceKeys.Length; j++)
                        {
                            serviceRegistry.RegisterType(resolvedServiceKeys[j], type);
                            Console.WriteLine($"[RegisterServicesFromAssembly] Registered service {resolvedServiceKeys[j]}: {type.FullName}");
                        }
                    }
                }
                else
                {
                    serviceRegistry.RegisterType(resolvedServiceKey, type);
                    Console.WriteLine($"[RegisterServicesFromAssembly] Registered service {resolvedServiceKey}: {type.FullName}");
                }
            }
        }
        #endregion
    }
}
