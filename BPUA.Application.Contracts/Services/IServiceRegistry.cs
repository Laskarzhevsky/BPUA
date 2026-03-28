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
        /// Checks whether service registry contains registered type or registered object
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <returns>True if registry contains registered type or registered object, otherwise False</returns>
        bool ContainsTypeOrObject(string registrationKey);

        /// <summary>
        /// Checks whether service registry contains registered use case state transitions
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <returns>True if registry contains registered use case state transitions</returns>
//        bool ContainsUseCaseStateTransitions(string registrationKey);

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
        /// Gets object of specified type
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="registrationKey">Registration key</param>
        /// <returns>object of specified type</returns>
        T GetObject<T>(string registrationKey);

        /// <summary>
        /// Gets use case state transition names
        /// IServiceRegistry interface implementation
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <returns>object of specified type</returns>
//        IList<string>? GetUseCaseStateTransitionNames(string registrationKey);

        /// <summary>
        /// Registers object
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="value">Object to register</param>
        void RegisterObject(string registrationKey, object value);

        /// <summary>
        /// Registers type
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="type">Type to register</param>
        void RegisterType(string registrationKey, Type type);

        /// <summary>
        /// Tries to get registered object
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="value">Retreived object</param>
        /// <returns>True if object retreived successully, otherwise False</returns>
        bool TryGetRegisteredObject(string registrationKey, out object? value);

        /// <summary>
        /// Tries to get registered type
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="registeredType">Registered type</param>
        /// <returns>True if type retreived successfully, otherwise False</returns>
        bool TryGetRegisteredType(string key, out Type registeredType);


        /// <summary>
        /// Determines whether the specified assembly has already been marked with the requested facet.
        /// </summary>
        /// <param name="assemblyFullName">Assembly full name.</param>
        /// <param name="facet">Assembly facet.</param>
        /// <returns>True when the assembly is known and already marked with the requested facet; otherwise False.</returns>
        bool HasAssemblyFacet(string assemblyFullName, AssemblyFacet facet);

        /// <summary>
        /// Tries to mark assembly facet
        /// </summary>
        /// <param name="assemblyFullName">Assembly full name</param>
        /// <param name="facet">Assembly facet</param>
        /// <returns>Action result</returns>
        bool TryMarkAssemblyFacet(string assemblyFullName, AssemblyFacet facet);

        /// <summary>
        /// Tries to register object
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="value">Object to register</param>
        /// <returns>True if object was registered, otherwise False</returns>
        public bool TryRegisterObject(string registrationKey, object value);

        /// <summary>
        /// Tries to registers transition name against state key
        /// </summary>
        /// <param name="registrationKey">Registration key</param>
        /// <param name="transitionName">Transition name</param>
        /// <returns>True if type was registered, otherwise False</returns>
//        bool TryRegisterTransitionNameAgainstStateKey(string registrationKey, string transitionName);

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
