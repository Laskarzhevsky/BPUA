using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Loader;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Process-local record of a successfully activated use case.
    /// Stored/cached inside BPUAApplication so repeated activations are no-ops.
    /// </summary>
    internal sealed class ActivationContext
    {
        // Identity / locator
        public string Key
        {
            get;
        }                   // normalized activation key (e.g., breadcrumbs [+ @version])
        public string DomainName
        {
            get;
        }
        public string UseCaseName
        {
            get;
        }
        public string Breadcrumbs
        {
            get;
        }           // as sent by the client; may be empty

        // Versioning (optional; fill when you add manifests)
        public string? Version
        {
            get;
        }
        public string? Hash
        {
            get;
        }

        // UI convenience (ignored by backend)
        public string? DefaultRoute
        {
            get;
        }

        // Diagnostics
        public DateTimeOffset ActivatedAt
        {
            get;
        }
        public AssemblyLoadContext? LoadContext
        {
            get;
        }

        // Assemblies
        public IReadOnlyList<Assembly> LoadedAssemblies
        {
            get
            {
                return _loadedAssemblies;
            }
        }
        public IReadOnlyList<Assembly> PageAssemblies
        {
            get
            {
                return _pageAssemblies;
            }
        }

        readonly ReadOnlyCollection<Assembly> _loadedAssemblies;
        readonly ReadOnlyCollection<Assembly> _pageAssemblies;

        /// <summary>
        /// Create a new activation context after a successful activation.
        /// </summary>
        internal ActivationContext(
            string key,
            string domainName,
            string useCaseName,
            string breadcrumbs,
            IEnumerable<Assembly>? loadedAssemblies,
            IEnumerable<Assembly>? pageAssemblies,
            string? defaultRoute,
            string? version,
            string? hash,
            AssemblyLoadContext? loadContext,
            DateTimeOffset activatedAtUtc)
        {
            // Minimal invariants (internal class, so we keep this simple)
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Activation key must be non-empty.", nameof(key));
            }
            if (domainName == null)
            {
                throw new ArgumentNullException(nameof(domainName));
            }
            if (useCaseName == null)
            {
                throw new ArgumentNullException(nameof(useCaseName));
            }
            if (breadcrumbs == null)
            {
                breadcrumbs = string.Empty;
            }

            Key = key;
            DomainName = domainName;
            UseCaseName = useCaseName;
            Breadcrumbs = breadcrumbs;
            DefaultRoute = defaultRoute;
            Version = version;
            Hash = hash;
            LoadContext = loadContext;

            if (activatedAtUtc == default)
            {
                ActivatedAt = DateTimeOffset.UtcNow;
            }
            else
            {
                ActivatedAt = activatedAtUtc;
            }

            _loadedAssemblies = ToReadOnlyList(loadedAssemblies);
            _pageAssemblies = ToReadOnlyList(pageAssemblies);
        }

        /// <summary>
        /// Helper to wrap an enumerable as a read-only list, filtering out nulls.
        /// </summary>
        static ReadOnlyCollection<Assembly> ToReadOnlyList(IEnumerable<Assembly>? source)
        {
            if (source == null)
            {
                return new List<Assembly>(capacity: 0).AsReadOnly();
            }

            List<Assembly> list = new List<Assembly>();
            foreach (Assembly a in source)
            {
                if (a != null)
                {
                    list.Add(a);
                }
            }
            return list.AsReadOnly();
        }

        /// <summary>
        /// Convenience factory for the common case when you don’t use a custom ALC and don’t have version/hash yet.
        /// </summary>
        internal static ActivationContext CreateBasic(
            string key,
            string domainName,
            string useCaseName,
            string breadcrumbs,
            IEnumerable<Assembly>? loadedAssemblies,
            IEnumerable<Assembly>? pageAssemblies,
            string? defaultRoute)
        {
            return new ActivationContext(
                key,
                domainName,
                useCaseName,
                breadcrumbs,
                loadedAssemblies,
                pageAssemblies,
                defaultRoute,
                version: null,
                hash: null,
                loadContext: null,
                activatedAtUtc: DateTimeOffset.UtcNow
            );
        }
    }
}
