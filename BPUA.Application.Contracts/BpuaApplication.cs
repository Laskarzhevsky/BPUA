using System;
using System.Threading;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Publishes the runtime application through its contract without an orchestration reference.
    /// The bootstrapper assigns the configured implementation before initializing services.
    /// </summary>
    public static class ManagementSystem
    {
        private static IBpuaApplication? _application;

        /// <summary>Gets whether an application has been assigned.</summary>
        public static bool IsInitialized
        {
            get { return Volatile.Read(ref _application) != null; }
        }

        /// <summary>Gets the application assigned during platform startup.</summary>
        public static IBpuaApplication Instance
        {
            get
            {
                IBpuaApplication? application = Volatile.Read(ref _application);
                if (application == null)
                {
                    throw new InvalidOperationException("BPUA management system has not been initialized. Boot the BPUA platform before accessing application services.");
                }
                return application;
            }
        }

        /// <summary>Assigns the runtime application once. Reassigning the same instance is harmless.</summary>
        public static void Initialize(IBpuaApplication application)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }
            IBpuaApplication? existing = Interlocked.CompareExchange(ref _application, application, null);
            if (existing != null && !object.ReferenceEquals(existing, application))
            {
                throw new InvalidOperationException("BPUA management system has already been initialized with another application.");
            }
        }
    }
}
