using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using BPUA.Application.Contracts;
using BPUA.Application.Services;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Dispatches events (instances of <see cref="EventArgs"/>) to all registered
    /// <see cref="BPUAService{TEventArgs}"/> handlers in the service registry.
    /// </summary>
    /// <remarks>
    /// This dispatcher:
    /// <list type="bullet">
    ///   <item><description>Does not use dependency injection — handlers are instantiated explicitly.</description></item>
    ///   <item><description>Ensures each handler is initialized, invoked, and disposed properly.</description></item>
    ///   <item><description>Matches handlers to events based on generic type compatibility.</description></item>
    /// </list>
    /// </remarks>
    public static class BPUAEventDispatcher
    {
        #region Public Methods
        /// <summary>
        /// Dispatches the specified event to all matching handlers in the <paramref name="serviceRegistry"/>.
        /// </summary>
        /// <typeparam name="TEvent">The event type to dispatch.</typeparam>
        /// <param name="e">The event instance.</param>
        /// <param name="serviceRegistry">The service registry containing handler types.</param>
        /// <returns>A task that completes once all handlers have processed the event.</returns>
        public static async Task Dispatch<TEvent>(TEvent e, IServiceRegistry serviceRegistry) where TEvent : EventArgs
        {
            IBPUAApplication app = BPUAApplication.GetInstance();

            // Enumerate all registered types
            IEnumerable<KeyValuePair<string, Type>> allTypes = serviceRegistry.EnumerateTypesByPrefix(string.Empty);
            foreach (KeyValuePair<string, Type> kv in allTypes)
            {
                Type serviceType = kv.Value;
                if (!typeof(IBPUAService).IsAssignableFrom(serviceType))
                {
                    continue;
                }

                if (!InheritsBpuAServiceOf<TEvent>(serviceType))
                {
                    continue;
                }

                IBPUAService? handler = Activator.CreateInstance(serviceType) as IBPUAService;
                if (handler == null)
                {
                    continue;
                }

                await handler.InitializeComponent(app);
                try
                {
                    await handler.HandleAsync(app, e).ConfigureAwait(false);
                }
                finally
                {
                    if (handler is IAsyncDisposable asyncDisposable)
                    {
                        await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                    }
                    else if (handler is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Determines whether a type inherits from <see cref="BPUAService{TEventArgs}"/>
        /// with a generic argument compatible with <typeparamref name="TEvent"/>.
        /// </summary>
        static bool InheritsBpuAServiceOf<TEvent>(Type type)
        {
            Type? t = type;
            while (t != null && t != typeof(object))
            {
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(BPUAService<>))
                {
                    Type[] args = t.GetGenericArguments();
                    return typeof(TEvent).IsAssignableFrom(args[0]);
                }

                t = t.BaseType;
            }

            return false;
        }
        #endregion
    }
}
