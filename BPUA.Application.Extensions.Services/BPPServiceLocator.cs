using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;
using BPUA.Core;

using System.Threading.Tasks;

namespace BPUA.Application.Extensions.Services
{
    /// <summary>
    /// Provides BPUA service locator functionality
    /// </summary>
    public static class BPUAServiceLocator
    {
        #region Methods
        /// <summary>
        /// Gets BPUA application
        /// </summary>
        /// <returns>BPUA application</returns>
        public static IBPUAApplication GetBPUAApplication()
        {
            IBPUAApplication bpuaApplication = BPUAApplication.GetInstance();
            return bpuaApplication;
        }

        /// <summary>
        /// Gets BPUA service
        /// </summary>
        /// <param name="bpuaServicekey">BPUA service key</param>
        /// <returns>BPUA service</returns>
        public static async Task<IBPUAService?> GetBPUAServiceAsync(string? bpuaServicekey)
        {
            if (string.IsNullOrEmpty(bpuaServicekey))
            {
                return null;
            }

            IBPUAApplication bppApplication = BPUAApplication.GetInstance();

            bpuaServicekey = bpuaServicekey.Trim('/');
            IBPUAIdentifier bpuaAIdentifier = new BPUAIdentifier(bpuaServicekey);
            UseCaseActivationResult useCaseActivationResult = await bppApplication.ActivateUseCaseAsync(bpuaAIdentifier);
            if (useCaseActivationResult.Succeeded)
            {
                return bppApplication.GetRequestHandler(bpuaServicekey);
            }

            return null;
        }
        #endregion
    }
}
