using System.Threading.Tasks;

using BPUA.Core;

namespace BPUA.Application.Contracts
{
    public interface IUseCaseActivator
    {
        /// <summary>
        /// Load and register assemblies for the requested use case on-demand.
        /// Must be idempotent at the level of the underlying loader/registrar.
        /// </summary>
        Task<UseCaseActivationResult> ActivateAsync(IBpuIdentifier identifier, IServiceRegistry registry);
    }
}
