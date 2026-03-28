using System;
using System.Collections.Generic;

using BPUA.Application.Contracts;
using BPUA.Core;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Single authoritative registry for runtime services and objects.
    /// Thread-safe for both type and object stores. Enumerations are snapshot-based.
    /// </summary>
    public sealed class ServiceRegistry : IServiceRegistry
    {
        readonly object _sync = new object();
        readonly Dictionary<string, Type> _registeredTypes = new Dictionary<string, Type>(StringComparer.Ordinal);
        readonly Dictionary<string, object> _registeredObjects = new Dictionary<string, object>(StringComparer.Ordinal);
        readonly Dictionary<string, AssemblyFacet> _assemblyIndex = new Dictionary<string, AssemblyFacet>(StringComparer.Ordinal);
//        readonly Dictionary<string, IList<string>> _registeredUseCaseStateTransitionNames = new Dictionary<string, IList<string>>(StringComparer.Ordinal);

        #region Public Methods
        /// <summary>
        /// Checks whether service registry contains registered type or registered object
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <returns>True if registry contains registered type or registered object, otherwise False</returns>
        public bool ContainsTypeOrObject(string registrationKey)
        {
            lock (_sync)
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
        }
/*
        /// <summary>
        /// Checks whether service registry contains registered use case state transitions
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <returns>True if registry contains registered use case state transitions</returns>
        public bool ContainsUseCaseStateTransitions(string registrationKey)
        {
            lock (_sync)
            {
                if (_registeredUseCaseStateTransitionNames.ContainsKey(registrationKey))
                {
                    return true;
                }

                return false;
            }
        }
*/
        /// <summary>
        /// Enumerates objects by prefix
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="prefix">Prefix</param>
        /// <returns>Enumerated objects by prefix</returns>
        public IEnumerable<KeyValuePair<string, object>> EnumerateObjectsByPrefix(string prefix)
        {
            // Snapshot under lock
            KeyValuePair<string, object>[] snapshot;
            lock (_sync)
            {
                snapshot = new KeyValuePair<string, object>[_registeredObjects.Count];
                int i = 0;
                foreach (KeyValuePair<string, object> keyValuePair in _registeredObjects)
                {
                    snapshot[i++] = keyValuePair;
                }
            }

            // Iterate snapshot outside the lock
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
            // Snapshot under lock
            KeyValuePair<string, Type>[] snapshot;
            lock (_sync)
            {
                snapshot = new KeyValuePair<string, Type>[_registeredTypes.Count];
                int i = 0;
                foreach (KeyValuePair<string, Type> keyValuePair in _registeredTypes)
                {
                    snapshot[i++] = keyValuePair;
                }
            }

            // Iterate snapshot outside the lock
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
        /// Gets object of specified type
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="registrationKey">Registration key</param>
        /// <returns>object of specified type</returns>
        public T GetObject<T>(string registrationKey)
        {
            lock (_sync)
            {
                return (T)_registeredObjects[registrationKey];
            }
        }
/*
        /// <summary>
        /// Gets use case state transition names
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <returns>List of use case state transition names</returns>
        public IList<string>? GetUseCaseStateTransitionNames(string registrationKey)
        {
            lock (_sync)
            {
                if (_registeredUseCaseStateTransitionNames.ContainsKey(registrationKey))
                {
                    return _registeredUseCaseStateTransitionNames[registrationKey];
                }

                return null;
            }
        }
*/
        /// <summary>
        /// Registers object
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="value">Object to register</param>
        public void RegisterObject(string registrationKey, object value)
        {
            lock (_sync)
            {
                _registeredObjects[registrationKey] = value;
            }
        }

        /// <summary>
        /// Registers type
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="type">Type to register</param>
        public void RegisterType(string registrationKey, Type type)
        {
            lock (_sync)
            {
                _registeredTypes[registrationKey] = type;
            }
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
            lock (_sync)
            {
                return _registeredObjects.TryGetValue(registrationKey, out value);
            }
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
            lock (_sync)
            {
                return _registeredTypes.TryGetValue(key, out registeredType!);
            }
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

            lock (_sync)
            {
                AssemblyFacet current;
                if (!_assemblyIndex.TryGetValue(assemblyFullName, out current))
                {
                    return false;
                }

                return (current & facet) == facet;
            }
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

            lock (_sync)
            {
                AssemblyFacet current;
                if (!_assemblyIndex.TryGetValue(assemblyFullName, out current))
                {
                    _assemblyIndex[assemblyFullName] = facet;
                    return true;
                }

                if ((current & facet) != 0)
                {
                    return false; // already handled
                }

                _assemblyIndex[assemblyFullName] = current | facet;
                return true;
            }
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
            lock (_sync)
            {
                if (_registeredObjects.ContainsKey(registrationKey))
                {
                    return false; // first-wins
                }

                _registeredObjects.Add(registrationKey, value);
                return true;
            }
        }
/*
        /// <summary>
        /// Tries to registers transition name against state key
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="transitionName">Transition name</param>
        /// <returns>True if type was registered, otherwise False</returns>
        public bool TryRegisterTransitionNameAgainstStateKey(string registrationKey, string transitionName)
        {
            lock (_sync)
            {
                if (_registeredUseCaseStateTransitionNames.ContainsKey(registrationKey))
                {
                    IList<string> transitionNames = _registeredUseCaseStateTransitionNames[registrationKey];
                    for (int i = 0; i < transitionNames.Count; i++)
                    {
                        if (transitionNames[i] == transitionName)
                        {
                            return false; // first-wins
                        }
                    }

                    transitionNames.Add(transitionName);
                }
                else
                {
                    IList<string> transitionNames = new List<string>();
                    transitionNames.Add(transitionName);
                    _registeredUseCaseStateTransitionNames.Add(registrationKey, transitionNames);
                }
                return true;
            }
        }
*/
        /// <summary>
        /// Tries to register type
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="type">Type to register</param>
        /// <returns>True if type was registered, otherwise False</returns>
        public bool TryRegisterType(string registrationKey, Type type)
        {
            lock (_sync)
            {
                if (_registeredTypes.ContainsKey(registrationKey))
                {
                    return false; // first-wins
                }

                _registeredTypes.Add(registrationKey, type);
                return true;
            }
        }
        #endregion
    }
}
