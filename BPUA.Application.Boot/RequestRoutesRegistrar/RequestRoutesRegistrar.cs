using System;
using System.Reflection;

using BPUA.Application.Contracts;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides BPUA request routes registrar functionality
    /// </summary>
    public static class RequestRoutesRegistrar
    {
        #region Public Methods
        /// <summary>
        /// Registers transitions from assembly
        /// </summary>
        /// <param name="loadedAssembly">Loaded assembly</param>
        /// <param name="serviceRegistry">Service registry</param>
        public static void RegisterTransitionsFromAssembly(Assembly loadedAssembly, IServiceRegistry serviceRegistry)
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

                if (!type.IsDefined(typeof(RegisterAsRequestRouteAttribute), inherit: false))
                {
                    continue;
                }

                string? resolvedRequestRouteKey = RequestRouteKeyResolver.TryToResolveRequestRouteKey(type);
                if (string.IsNullOrEmpty(resolvedRequestRouteKey))
                {
                    resolvedRequestRouteKey = RequestRouteKeyResolver.TryToBuildRequestRouteKeyFromIdentification(type);
                }

                if (string.IsNullOrEmpty(resolvedRequestRouteKey))
                {
                    string? key = type.FullName;
                    if (!string.IsNullOrEmpty(key))
                    {
                        serviceRegistry.RegisterRequestRouteType(key, type);
                        Console.WriteLine("[RegisterRequestRoutesFromAssembly] Registered request route " + key + ": " + type.FullName);
                    }

                    continue;
                }

                serviceRegistry.RegisterRequestRouteType(resolvedRequestRouteKey, type);
                Console.WriteLine("[RegisterRequestRoutesFromAssembly] Registered request route " + resolvedRequestRouteKey + ": " + type.FullName);
            }
        }
        #endregion
    }
}
