using BPUA.Application.Contracts;
using BPUA.Core;

using System;
using System.Threading.Tasks;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Provides state machine functionality
    /// </summary>
    public class StateMachine
    {
        #region Methods
        /// <summary>
        /// Starts state machine
        /// </summary>
        /// <param name="bpuaApplication">BPUA application instance</param>
        /// <param name="bpuIdentifier">BPU identifier</param>
        public async Task ExecuteTransition(BpuaApplication bpuaApplication, IBpuIdentifier bpuIdentifier)
        {
            UseCaseActivationResult useCaseActivationResult = await bpuaApplication.ActivateUseCaseAsync(bpuIdentifier);
            if (useCaseActivationResult.Succeeded)
            {
                string bpuaServicekey = KeyCompiler.CompileStateHandlerKey(bpuIdentifier.DomainName, bpuIdentifier.UseCaseName, bpuIdentifier.ApplicationLayerName, bpuIdentifier.StateName);
                IBpuaService? bpuaService = bpuaApplication.GetRequestHandler(bpuaServicekey);
                if (bpuaService == null)
                {
                    throw new InvalidOperationException($"State handler with key '{bpuaServicekey}' is not found for hosted application layer with key '{KeyCompiler.CompileHostedApplicationLayerKey(bpuIdentifier.DomainName, bpuIdentifier.UseCaseName, bpuIdentifier.ApplicationLayerName)}'.");
                }
                else
                {
                    if (bpuaService is IStateHandler)
                    {
                        IStateHandler stateHandler = (IStateHandler)bpuaService;
                        await stateHandler.Initialize();
                        if (bpuIdentifier.StateName != stateHandler.BpuIdentifier.StateName)
                        {
                            await ExecuteTransition(bpuaApplication, stateHandler.BpuIdentifier);
                        }
                    }
                }
            }
            else
            {
                throw new InvalidOperationException($"Use case activation failed for BPU identifier '{bpuIdentifier}'.");
            }
        }
        #endregion
    }
}
