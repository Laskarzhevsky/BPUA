using System;
using System.Collections.Generic;
using System.Linq;

using BPUA.Application.Contracts;
using BPUA.Application.Services;

namespace BPUA.Application.Orchestration
{
    public static class ServiceRegistryActivationExtensions
    {
        // Lookup by explicit key or by stem (with prefix + heuristic fallback).
        public static IBpuaService? GetBpuaService(this IServiceRegistry registry, string key)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            // 1) Exact key
            Type exactType;
            if (registry.TryGetRegisteredType(key, out exactType))
                return CreateAndInit(exactType);

            // 2) Prefix search
            string prefix = key + "/";
            List<KeyValuePair<string, Type>> matches = new List<KeyValuePair<string, Type>>();
            IEnumerable<KeyValuePair<string, Type>> all = registry.EnumerateTypesByPrefix(prefix);

            foreach (KeyValuePair<string, Type> kv in all)
            {
                matches.Add(kv);
            }

            if (matches.Count == 0)
                return null;

            if (matches.Count == 1)
                return CreateAndInit(matches[0].Value);

            // 3) Disambiguate: prefer type name ending with "*{stem}EventHandler"
            string preferredSuffix = key + "EventHandler";
            Type? preferred = null;

            for (int i = 0; i < matches.Count; i++)
            {
                Type t = matches[i].Value;
                if (t.Name.EndsWith(preferredSuffix, StringComparison.Ordinal))
                {
                    preferred = t;
                    break;
                }
            }

            if (preferred != null)
                return CreateAndInit(preferred);

            // Still ambiguous → let the caller publish via dispatcher
            return null;
        }

        // By EventArgs instance → derive stem → delegate to the string overload
        public static IBpuaService? GetBpuaService(this IServiceRegistry registry, EventArgs args)
        {
            string stem = DeriveKeyFromEventArgs(args.GetType()); // e.g., "RequestToNextLayer"
            return registry.GetBpuaService(stem);
        }

        // By EventArgs type → derive stem → delegate
        public static IBpuaService? GetBpuaService(this IServiceRegistry registry, Type eventArgsType)
        {
            string stem = DeriveKeyFromEventArgs(eventArgsType);
            return registry.GetBpuaService(stem);
        }

        static string DeriveKeyFromEventArgs(Type t)
        {
            const string suffix = "EventArgs";
            string name = t.Name;
            if (name.EndsWith(suffix, StringComparison.Ordinal))
                return name.Substring(0, name.Length - suffix.Length);
            return name;
        }

        static IBpuaService? CreateAndInit(Type type)
        {
            var instance = Activator.CreateInstance(type) as IBpuaService;
            if (instance != null)
            {
                instance.InitializeComponent(BpuaApplication.GetInstance());
            }
            return instance;
        }
    }
}
