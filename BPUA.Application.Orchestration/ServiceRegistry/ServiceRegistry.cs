using BPUA.Application.Contracts;
using BPUA.Core;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Single authoritative registry for runtime services and objects.
    /// Thread-safe for both type and object stores. Enumerations are snapshot-based.
    /// </summary>
    public sealed class ServiceRegistry : IServiceRegistry
    {
        readonly ConcurrentDictionary<string, string> _dynamicAssembliesPathIndex = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        readonly ConcurrentDictionary<string, Type> _registeredTypes = new ConcurrentDictionary<string, Type>(StringComparer.Ordinal);
        readonly ConcurrentDictionary<string, object> _registeredObjects = new ConcurrentDictionary<string, object>(StringComparer.Ordinal);
        readonly ConcurrentDictionary<string, AssemblyFacet> _assemblyIndex = new ConcurrentDictionary<string, AssemblyFacet>(StringComparer.Ordinal);

        #region Public Methods
        /// <summary>
        /// Checks whether dynamic assemblies path index contains assembly name
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="assemblyName">Assembly name</param>
        /// <returns>True if dynamic assemblies path index contains assembly name, otherwise False</returns>
        public bool ContainsDynamicAssemblyName(string assemblyName)
        {
            return _dynamicAssembliesPathIndex.ContainsKey(assemblyName);
        }

        /// <summary>
        /// Checks whether service registry contains registered type or registered object
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <returns>True if registry contains registered type or registered object, otherwise False</returns>
        public bool ContainsTypeOrObject(string registrationKey)
        {
            if (_registeredTypes.ContainsKey(registrationKey))
            {
                return true;
            }

            if (_registeredObjects.ContainsKey(registrationKey))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Enumerates objects by prefix
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="prefix">Prefix</param>
        /// <returns>Enumerated objects by prefix</returns>
        public IEnumerable<KeyValuePair<string, object>> EnumerateObjectsByPrefix(string prefix)
        {
            KeyValuePair<string, object>[] snapshot = _registeredObjects.ToArray();

            for (int i = 0; i < snapshot.Length; i++)
            {
                KeyValuePair<string, object> keyValuePair = snapshot[i];
                if (string.IsNullOrEmpty(prefix) || keyValuePair.Key.StartsWith(prefix, StringComparison.Ordinal))
                {
                    yield return keyValuePair;
                }
            }
        }

        /// <summary>
        /// Enumerates types by prefix
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <returns>Enumerated types by prefix</returns>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, Type>> EnumerateTypesByPrefix(string prefix)
        {
            KeyValuePair<string, Type>[] snapshot = _registeredTypes.ToArray();

            for (int i = 0; i < snapshot.Length; i++)
            {
                KeyValuePair<string, Type> keyValuePair = snapshot[i];
                if (string.IsNullOrEmpty(prefix) || keyValuePair.Key.StartsWith(prefix, StringComparison.Ordinal))
                {
                    yield return keyValuePair;
                }
            }
        }

        /// <summary>
        /// Gets dynamic assembly path
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="assemblyName">Assembly name</param>
        /// <returns>Dynamic assembly path</returns>
        public string GetDynamicAssemblyPath(string assemblyName)
        {
            return _dynamicAssembliesPathIndex[assemblyName];
        }

        /// <summary>
        /// Gets object of specified type
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="registrationKey">Registration key</param>
        /// <returns>object of specified type</returns>
        public T GetObject<T>(string registrationKey)
        {
            return (T)_registeredObjects[registrationKey];
        }

        /// <summary>
        /// Determines whether the specified assembly has already been marked with the requested facet.
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="assemblyFullName">Assembly full name</param>
        /// <param name="facet">Assembly facet</param>
        /// <returns>True when the assembly is known and already marked with the requested facet; otherwise False.</returns>
        public bool HasAssemblyFacet(string assemblyFullName, AssemblyFacet facet)
        {
            if (string.IsNullOrEmpty(assemblyFullName))
            {
                return false;
            }

            AssemblyFacet current;
            if (!_assemblyIndex.TryGetValue(assemblyFullName, out current))
            {
                return false;
            }

            return (current & facet) == facet;
        }

        /// <summary>
        /// Registers dynamic assembly path
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="assemblyName">Assembly name</param>
        /// <param name="assemblyPath">Assembly path</param>
        public void RegisterDynamicAssemblyPath(string assemblyName, string assemblyPath)
        {
            _dynamicAssembliesPathIndex[assemblyName] = assemblyPath;
        }

        /// <summary>
        /// Registers object
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="value">Object to register</param>
        public void RegisterObject(string registrationKey, object value)
        {
            _registeredObjects[registrationKey] = value;
        }

        /// <summary>
        /// Registers type
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="type">Type to register</param>
        public void RegisterType(string registrationKey, Type type)
        {
            _registeredTypes[registrationKey] = type;
        }

        /// <summary>
        /// Tries to get registered dynamic assembly path
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="assemblyName">Assembly name</param>
        /// <param name="assemblyPath">Registered assembly path</param>
        /// <returns>True if assembly path was retrieved successfully, otherwise False</returns>
        public bool TryGetRegisteredDynamicAssemblyPath(string assemblyName, out string assemblyPath)
        {
            return _dynamicAssembliesPathIndex.TryGetValue(assemblyName, out assemblyPath!);
        }

        /// <summary>
        /// Tries to get registered object
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="value">Retreived object</param>
        /// <returns>True if object retreived successully, otherwise False</returns>
        public bool TryGetRegisteredObject(string registrationKey, out object? value)
        {
            return _registeredObjects.TryGetValue(registrationKey, out value);
        }

        /// <summary>
        /// Tries to get registered type
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="registeredType">Registered type</param>
        /// <returns>True if type retreived successfully, otherwise False</returns>
        public bool TryGetRegisteredType(string key, out Type registeredType)
        {
            return _registeredTypes.TryGetValue(key, out registeredType!);
        }

        /// <summary>
        /// Tries to mark assembly facet
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="assemblyFullName">Assembly full name</param>
        /// <param name="facet">Assembly facet</param>
        /// <returns>Action result</returns>
        public bool TryMarkAssemblyFacet(string assemblyFullName, AssemblyFacet facet)
        {
            if (string.IsNullOrEmpty(assemblyFullName))
            {
                return false;
            }

            while (true)
            {
                AssemblyFacet current;
                if (_assemblyIndex.TryGetValue(assemblyFullName, out current))
                {
                    if ((current & facet) == facet)
                    {
                        return false;
                    }

                    AssemblyFacet updated = current | facet;
                    if (_assemblyIndex.TryUpdate(assemblyFullName, updated, current))
                    {
                        return true;
                    }

                    continue;
                }

                if (_assemblyIndex.TryAdd(assemblyFullName, facet))
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Tries to registers dynamic assembly path
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="assemblyName">Assembly name</param>
        /// <param name="assemblyPath">Assembly path</param>
        public bool TryRegisterDynamicAssemblyPath(string assemblyName, string assemblyPath)
        {
            return _dynamicAssembliesPathIndex.TryAdd(assemblyName, assemblyPath);
        }

        /// <summary>
        /// Tries to register object
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="value">Object to register</param>
        /// <returns>True if object was registered, otherwise False</returns>
        public bool TryRegisterObject(string registrationKey, object value)
        {
            return _registeredObjects.TryAdd(registrationKey, value);
        }

        /// <summary>
        /// Tries to register type
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="type">Type to register</param>
        /// <returns>True if type was registered, otherwise False</returns>
        public bool TryRegisterType(string registrationKey, Type type)
        {
            return _registeredTypes.TryAdd(registrationKey, type);
        }
        #endregion
    }
}
