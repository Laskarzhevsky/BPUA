using System;
using System.Collections.Generic;

namespace BPUA.Application.Contracts
{
    /// <summary>Result of a use-case activation request.</summary>
    public class UseCaseActivationResult
    {
        /// <summary>True if activation is usable (either newly activated or already loaded).</summary>
        public bool Succeeded
        {
            get; set;
        }

        /// <summary>True if the use case was already active and no load was needed.</summary>
        public bool NoAdditionalAssembliesWereLoaded
        {
            get; set;
        }

        /// <summary>Route the UI should navigate to after activation; null for non-UI services.</summary>
        public string? DefaultRoute
        {
            get; set;
        }

        /// <summary>Non-empty only when <see cref="Succeeded"/> is false (diagnostics for the caller).</summary>
        public List<string> Errors { get; } = new List<string>();
    }
}
