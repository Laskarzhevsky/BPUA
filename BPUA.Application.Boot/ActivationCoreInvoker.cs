using BPUA.Application.Contracts;
using BPUA.Core;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides a concrete non-lambda callback for Lazy activation execution.
    /// The class captures a snapshot of the incoming identifier so delayed execution
    /// is isolated from later external mutations of the original identifier instance.
    /// </summary>
    sealed class ActivationCoreInvoker
    {
        readonly UseCaseActivator _useCaseActivator;
        readonly IdentifierSnapshot _identifierSnapshot;
        readonly IServiceRegistry _serviceRegistry;

        /// <summary>
        /// Initializes the invoker with the owning activator, the identifier to snapshot,
        /// and the registry that will receive loaded services.
        /// </summary>
        /// <param name="owner">The activator that performs the real activation work.</param>
        /// <param name="identifier">The identifier that describes the use case to activate.</param>
        /// <param name="serviceRegistry">The registry used during activation.</param>
        public ActivationCoreInvoker(UseCaseActivator owner, IBpuIdentifier identifier, IServiceRegistry serviceRegistry)
        {
            _useCaseActivator = owner;
            _serviceRegistry = serviceRegistry;
            _identifierSnapshot = new IdentifierSnapshot(identifier);
        }

        /// <summary>
        /// Invokes the owning activator using the stored identifier snapshot and service registry.
        /// This method exists specifically so Lazy can receive a method group instead of a lambda expression.
        /// </summary>
        /// <returns>The activation result produced by the owning activator.</returns>
        public UseCaseActivationResult Invoke()
        {
            return _useCaseActivator.ActivateCore(_identifierSnapshot, _serviceRegistry);
        }
    }
}
