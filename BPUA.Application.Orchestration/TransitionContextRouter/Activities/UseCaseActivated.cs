using BPUA.Application.Contracts;
using BPUA.Core;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.IData;

using System;
using System.Threading.Tasks;
using System.Transactions;

namespace BPUA.Application.Orchestration
{
    /// <summary>
    /// Provides request to the next layer event handler functionality
    /// </summary>
    public partial class TransitionContextRouter
    {
        #region Private Methods
        /// <summary>
        /// Checks if the use case specified in the BPUA identifier is activated, and if not, activates it
        /// </summary>
        /// <returns>True if the use case is activated; otherwise, false.</returns>
        async Task<bool> UseCaseActivated()
        {
            UseCaseActivationResult useCaseActivationResult = await ((BPUAApplication)BpuaApplication!).ActivateUseCaseAsync(BpuaIdentifier);
            return useCaseActivationResult.Succeeded;
        }
        #endregion
    }
}
