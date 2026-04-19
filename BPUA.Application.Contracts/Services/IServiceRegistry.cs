using System;
using System.Collections.Generic;

using BPUA.Core;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Canonical runtime registry abstraction used across the platform.
    /// Holds Type registrations and arbitrary objects under string keys.
    /// </summary>
    public interface IServiceRegistry
    {
        #region Types Related Methods
        /// <summary>
        /// Checks whether dynamic assemblies path index contains assembly name
        /// </summary>
        /// <param name="assemblyName">Assembly name</param>
        /// <returns>True if dynamic assemblies path index contains assembly name, otherwise False</returns>
        bool ContainsDynamicAssemblyName(string assemblyName);

        /// <summary>
        /// Checks whether service registry contains registered type or registered object
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <returns>True if registry contains registered type or registered object, otherwise False</returns>
        bool ContainsTypeOrObject(string registrationKey);

        /// <summary>
        /// Checks whether service registry contains registered transition type
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <returns>True if registry contains registered transition type, otherwise False</returns>
        bool ContainsTransitionType(string registrationKey);

        /// <summary>
        /// Enumerates objects by prefix
        /// </summary>
        /// <param name="prefix">Prefix for enumeration</param>
        /// <returns>Enumerated objects by prefix</returns>
        IEnumerable<KeyValuePair<string, object>> EnumerateObjectsByPrefix(string prefix);

        /// <summary>
        /// Enumerates types by prefix
        /// </summary>
        /// <param name="prefix">Prefix for enumeration</param>
        /// <returns>Enumerated types by prefix</returns>
        IEnumerable<KeyValuePair<string, Type>> EnumerateTypesByPrefix(string prefix);

        /// <summary>
        /// Gets dynamic assembly path
        /// </summary>
        /// <param name="assemblyName">Assembly name</param>
        /// <returns>Dynamic assembly path</returns>
        string GetDynamicAssemblyPath(string assemblyName);

        /// <summary>
        /// Gets object of specified type
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="registrationKey">Registration key</param>
        /// <returns>object of specified type</returns>
        T GetObject<T>(string registrationKey);

        /// <summary>
        /// Determines whether the specified assembly has already been marked with the requested facet.
        /// </summary>
        /// <param name="assemblyFullName">Assembly full name.</param>
        /// <param name="facet">Assembly facet.</param>
        /// <returns>True when the assembly is known and already marked with the requested facet; otherwise False.</returns>
        bool HasAssemblyFacet(string assemblyFullName, AssemblyFacet facet);

        /// <summary>
        /// Registers dynamic assembly path
        /// </summary>
        /// <param name="assemblyName">Assembly name</param>
        /// <param name="assemblyPath">Assembly path</param>
        void RegisterDynamicAssemblyPath(string assemblyName, string assemblyPath);

        /// <summary>
        /// Registers object
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="value">Object to register</param>
        void RegisterObject(string registrationKey, object value);

        /// <summary>
        /// Registers transition type
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="transitionType">Transition type</param>
        void RegisterTransitionType(string registrationKey, Type transitionType);

        /// <summary>
        /// Registers type
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="type">Type to register</param>
        void RegisterType(string registrationKey, Type type);

        /// <summary>
        /// Tries to get registered dynamic assembly path
        /// </summary>
        /// <param name="assemblyName">Assembly name</param>
        /// <param name="assemblyPath">Registered assembly path</param>
        /// <returns>True if assembly path was retrieved successfully, otherwise False</returns>
        bool TryGetRegisteredDynamicAssemblyPath(string assemblyName, out string assemblyPath);

        /// <summary>
        /// Tries to get registered object
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="value">Retreived object</param>
        /// <returns>True if object retreived successully, otherwise False</returns>
        bool TryGetRegisteredObject(string registrationKey, out object? value);

        /// <summary>
        /// Tries to get registered transition type
        /// </summary>
        /// <param name="bpuaIdentifier">BPUA identifier</param>
        /// <param name="registeredTransitionType">Registered transition type</param>
        /// <returns>True if type retreived successfully, otherwise False</returns>
        bool TryGetRegisteredTransitionType(IBPUAIdentifier bpuaIdentifier, out Type registeredTransitionType);

        /// <summary>
        /// Tries to get registered type
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="registeredType">Registered type</param>
        /// <returns>True if type retreived successfully, otherwise False</returns>
        bool TryGetRegisteredType(string key, out Type registeredType);

        /// <summary>
        /// Tries to mark assembly facet
        /// </summary>
        /// <param name="assemblyFullName">Assembly full name</param>
        /// <param name="facet">Assembly facet</param>
        /// <returns>Action result</returns>
        bool TryMarkAssemblyFacet(string assemblyFullName, AssemblyFacet facet);

        /// <summary>
        /// Tries to registers dynamic assembly path
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="assemblyName">Assembly name</param>
        /// <param name="assemblyPath">Assembly path</param>
        bool TryRegisterDynamicAssemblyPath(string assemblyName, string assemblyPath);

        /// <summary>
        /// Tries to register object
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="value">Object to register</param>
        /// <returns>True if object was registered, otherwise False</returns>
        public bool TryRegisterObject(string registrationKey, object value);

        /// <summary>
        /// Tries to register transition type
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="transitionType">Transition type</param>
        /// <returns>True if transition type was registered, otherwise False</returns>
        bool TryRegisterTransitionType(string registrationKey, Type transitionType);

        /// <summary>
        /// Tries to register type
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="type">Type to register</param>
        /// <returns>True if type was registered, otherwise False</returns>
        bool TryRegisterType(string registrationKey, Type type);
        #endregion
    }
}
