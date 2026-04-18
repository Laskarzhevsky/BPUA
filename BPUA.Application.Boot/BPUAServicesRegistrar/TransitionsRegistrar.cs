using System;
using System.Reflection;

using BPUA.Application.Contracts;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides BPUA transitions registrar functionality
    /// </summary>
    public static class TransitionsRegistrar
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

                if (!type.IsDefined(typeof(RegisterAsTransitionAttribute), inherit: false))
                {
                    continue;
                }

                string? resolvedTransitionKey = TransitionKeyResolver.TryToResolveTransitionKey(type);
                if (string.IsNullOrEmpty(resolvedTransitionKey))
                {
                    resolvedTransitionKey = TransitionKeyResolver.TryToBuildTransitionKeyFromIdentification(type);
                }

                if (string.IsNullOrEmpty(resolvedTransitionKey))
                {
                    string? key = type.FullName;
                    if (!string.IsNullOrEmpty(key))
                    {
                        serviceRegistry.RegisterTransitionType(key, type);
                        Console.WriteLine("[RegisterTransitionsFromAssembly] Registered transition " + key + ": " + type.FullName);
                    }

                    continue;
                }

                serviceRegistry.RegisterTransitionType(resolvedTransitionKey, type);
                Console.WriteLine("[RegisterTransitionsFromAssembly] Registered transition " + resolvedTransitionKey + ": " + type.FullName);
            }
        }
        #endregion
    }
}
