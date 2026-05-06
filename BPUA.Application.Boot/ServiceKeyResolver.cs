using System;
using System.Collections.Generic;
using System.Reflection;

using BPUA.Application.Services;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides service key resolver functionality
    /// </summary>
    internal static class ServiceKeyResolver
    {
        #region Public Methods
        /// <summary>
        /// Tries to resolve service key
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Service key</returns>
        public static string? TryToResolveServiceKey(Type type)
        {
            PropertyInfo? serviceKeyPropertyInfo = type.GetProperty("ServiceKey", BindingFlags.Public | BindingFlags.Static);
            if (serviceKeyPropertyInfo?.PropertyType == typeof(string))
            {
                object? value = serviceKeyPropertyInfo.GetValue(null);
                if (value is string serviceKey && !string.IsNullOrWhiteSpace(serviceKey))
                {
                    return serviceKey;
                }
            }

            return null;
        }

        /// <summary>
        /// Tries to resolve service key
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Service key</returns>
        public static string[]? TryToResolveServiceKeys(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            PropertyInfo? serviceKeysPropertyInfo = type.GetProperty("ServiceKeys", BindingFlags.Public | BindingFlags.Static);
            if (serviceKeysPropertyInfo != null)
            {
                object? value = serviceKeysPropertyInfo.GetValue(null);
                if (value != null)
                {
                    // Case 1: string[]
                    if (serviceKeysPropertyInfo.PropertyType == typeof(string[]) && value is string[] arr)
                    {
                        return NormalizeCopy(arr);
                    }

                    // Case 2: IReadOnlyList<string>
                    if (value is IReadOnlyList<string> roList)
                    {
                        return NormalizeCopy(roList);
                    }

                    // Case 3: IEnumerable<string>
                    if (value is IEnumerable<string> seq)
                    {
                        return NormalizeCopy(seq);
                    }
                }
            }

            // Fallback: single ServiceKey -> array of one
            string? single = TryToResolveServiceKey(type);
            if (!string.IsNullOrWhiteSpace(single))
            {
                return new string[] { single! };
            }

            return null;
        }

        public static string? TryToResolveEventArgsKey(Type type)
        {
            // Priority 3: derives from BpuaService<TEventArgs>
            // -> create a unique key per handler type for the same event
            Type? baseType = type.BaseType;
            while (baseType != null && baseType != typeof(object))
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(BpuaService<>))
                {
                    Type eventArgsType = baseType.GetGenericArguments()[0];
                    string stem = eventArgsType.Name;
                    if (stem.EndsWith("EventArgs", StringComparison.Ordinal))
                    {
                        stem = stem.Substring(0, stem.Length - "EventArgs".Length);
                    }

                    // Include handler type to ensure uniqueness
                    string handlerId = type.FullName ?? type.Name;
                    return $"{stem}/{handlerId}";
                }

                baseType = baseType.BaseType;
            }

            // Fallback: fully-qualified type name
            return type.FullName;
        }
        #endregion

        #region Methods
        static string[]? NormalizeCopy(IEnumerable<string> source)
        {
            List<string> list = new List<string>();
            foreach (string? item in source)
            {
                if (item == null)
                {
                    continue;
                }

                string trimmed = item.Trim();
                if (trimmed.Length == 0)
                {
                    continue;
                }

                bool exists = false;
                int i = 0;
                while (i < list.Count)
                {
                    if (string.Equals(list[i], trimmed, StringComparison.Ordinal))
                    {
                        exists = true;
                        break;
                    }
                    i = i + 1;
                }

                if (!exists)
                {
                    list.Add(trimmed);
                }
            }

            if (list.Count == 0)
            {
                return null;
            }

            return list.ToArray();
        }

        static string[]? NormalizeCopy(IReadOnlyList<string> source)
        {
            // Forward to IEnumerable<string> implementation
            return NormalizeCopy((IEnumerable<string>)source);
        }

        static string[]? NormalizeCopy(string[] source)
        {
            // Forward to IEnumerable<string> implementation
            return NormalizeCopy((IEnumerable<string>)source);
        }
        #endregion
    }
}
