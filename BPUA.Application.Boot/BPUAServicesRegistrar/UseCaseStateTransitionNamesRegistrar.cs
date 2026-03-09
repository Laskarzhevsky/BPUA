using BPUA.Application.Contracts;
using BPUA.Core;

namespace BPUA.Application.Boot
{
    /// <summary>
    /// Provides use case state transition names registrar
    /// </summary>
    internal static class UseCaseStateTransitionNamesRegistrar
    {
        public static void RegisterUseCaseStateTransitionNames(string serviceKey, IServiceRegistry serviceRegistry)
        {
            BPUAIdentifier bpuaIdentifier = new BPUAIdentifier(serviceKey);
            if (string.IsNullOrEmpty(bpuaIdentifier.TransitionName))
            {
                return;
            }

            if (bpuaIdentifier.ApplicationLayerName == BPUA.Application.Contracts.ApplicationLayersNames.BL)
            {
                string stateHandlerKey = KeyCompiler.CompileStateHandlerKey(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName, bpuaIdentifier.StateName);
                serviceRegistry.TryRegisterTransitionNameAgainstStateKey(stateHandlerKey, bpuaIdentifier.TransitionName);
            }
        }
    }
}
