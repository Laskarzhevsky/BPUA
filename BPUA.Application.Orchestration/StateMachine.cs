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
        /// <param name="bpuaIdentifier">BPUA identifier</param>
        public async Task ExecuteTransition(BPUAApplication bpuaApplication, IBPUAIdentifier bpuaIdentifier)
        {
            UseCaseActivationResult useCaseActivationResult = await bpuaApplication.ActivateUseCaseAsync(bpuaIdentifier);
            if (useCaseActivationResult.Succeeded)
            {
                string bpuaServicekey = KeyCompiler.CompileStateHandlerKey(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName, bpuaIdentifier.StateName);
                IBPUAService? bpuaService = bpuaApplication.GetRequestHandler(bpuaServicekey);
                if (bpuaService == null)
                {
                    throw new InvalidOperationException($"State handler with key '{bpuaServicekey}' is not found for hosted application layer with key '{KeyCompiler.CompileHostedApplicationLayerKey(bpuaIdentifier.DomainName, bpuaIdentifier.UseCaseName, bpuaIdentifier.ApplicationLayerName)}'.");
                }
                else
                {
                    if (bpuaService is IStateHandler)
                    {
                        IStateHandler stateHandler = (IStateHandler)bpuaService;
                        await stateHandler.Initialize();
                        if (bpuaIdentifier.StateName != stateHandler.BpuaIdentifier.StateName)
                        {
                            await ExecuteTransition(bpuaApplication, stateHandler.BpuaIdentifier);
                        }
                    }
                }
            }
            else
            {
                throw new InvalidOperationException($"Use case activation failed for BPUA identifier '{bpuaIdentifier}'.");
            }
        }
        #endregion
    }
}
