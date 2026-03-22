/*
using BPUA.Application.Contracts;

using System;
using System.Collections.Generic;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Provides transition registry functionality.
    /// </summary>
    public class TransitionRegistry : ITransitionRegistry
    {
        #region Public Methods
        /// <summary>
        /// Registers transition
        /// </summary>
        /// <param name="transitionForRegistration">Transition for registration</param>
        public void RegisterTransition(ITransition transitionForRegistration)
        {
            if (transitionForRegistration == null)
            {
                return;
            }

            _transitions.Add(transitionForRegistration);
        }

        /// <summary>
        /// Geta transition
        /// </summary>
        /// <param name="requestorTypeFullName">Requestor type full name</param>
        /// <param name="eventName">Event name</param>
        /// <param name="transitionName">Transition name</param>
        /// <returns></returns>
        public ITransition? GetTransition(string requestorTypeFullName, string eventName, string? transitionName)
        {
            ITransition? transitionDefinition = null;
            int i = 0;

            for (i = 0; i < _transitions.Count; i++)
            {
                transitionDefinition = _transitions[i];

                if (!string.Equals(transitionDefinition.Key.RequestorTypeFullName, requestorTypeFullName, StringComparison.Ordinal))
                {
                    continue;
                }

                if (!string.Equals(transitionDefinition.Key.EventName, eventName, StringComparison.Ordinal))
                {
                    continue;
                }

                if (!string.Equals(transitionDefinition.Key.RequestedTransitionName, transitionName, StringComparison.Ordinal))
                {
                    continue;
                }

                return transitionDefinition;
            }

            return null;
        }
        #endregion
    }
}
*/