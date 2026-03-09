using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using BPUA.Application.Contracts;
using BPUA.Application.Services;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Scans assemblies for IBPUAService implementations and registers them using ServiceKey or EventArgs generic type
    /// </summary>
    public static class BPUAServicesScanner
    {
        public static void ScanAndRegisterServices(string pluginFolderPath, IDictionary<string, Type> registry)
        {
            if (!Directory.Exists(pluginFolderPath))
            {
                return;
            }

            string[] dllPaths = Directory.GetFiles(pluginFolderPath, "*.dll");
            foreach (string dllPath in dllPaths)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(dllPath);
                    object[] attrs = assembly.GetCustomAttributes(typeof(RegisterAsBPUAServiceAssemblyAttribute), inherit: false);
                    if (attrs == null || attrs.Length == 0)
                    {
                        continue;
                    }

                    foreach (Type type in assembly.GetTypes())
                    {
                        if (!type.IsClass || type.IsAbstract)
                            continue;

                        if (!typeof(IBPUAService).IsAssignableFrom(type))
                            continue;

                        string? key = null;

                        // Priority 1: Check static ServiceKey property
                        PropertyInfo? keyProp = type.GetProperty("ServiceKey", BindingFlags.Public | BindingFlags.Static);
                        if (keyProp != null && keyProp.PropertyType == typeof(string))
                        {
                            object? value = keyProp.GetValue(null);
                            if (value is string serviceKey && !string.IsNullOrWhiteSpace(serviceKey))
                            {
                                key = serviceKey;
                            }
                        }

                        // Priority 2: Check generic parameter of base class
                        if (string.IsNullOrWhiteSpace(key))
                        {
                            Type? baseType = type.BaseType;
                            while (baseType != null)
                            {
                                if (baseType.IsGenericType &&
                                    baseType.GetGenericTypeDefinition() == typeof(BPUAService<>))
                                {
                                    Type eventArgsType = baseType.GetGenericArguments()[0];
                                    key = eventArgsType.Name;
                                    key = key.Substring(0, key.Length - "EventArgs".Length);
                                    break;
                                }

                                baseType = baseType.BaseType;
                            }
                        }

                        // Fallback: use full name
                        if (string.IsNullOrWhiteSpace(key))
                        {
                            key = type.FullName;
                        }

                        registry[key] = type;
                    }
                }
                catch (ReflectionTypeLoadException reflectionTypeLoadException)
                {
                    string error = reflectionTypeLoadException.Message;
                    // Optional: log loader issues
                }
                catch (Exception exception)
                {
                    string error = exception.Message;
                    // Optional: log general failures
                }
            }
        }
    }
}
