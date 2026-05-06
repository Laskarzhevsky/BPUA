using BPUA.Application.Contracts;
using BPUA.Application.Orchestration;
using BPUA.Core;

using System.Threading.Tasks;

namespace BPUA.Application.Extensions.Services
{
    /// <summary>
    /// Provides BPUA service locator functionality
    /// </summary>
    public static class BpuaServiceLocator
    {
        #region Methods
        /// <summary>
        /// Gets BPUA application
        /// </summary>
        /// <returns>BPUA application</returns>
        public static IBpuaApplication GetBpuaApplication()
        {
            IBpuaApplication bpuaApplication = BpuaApplication.GetInstance();
            return bpuaApplication;
        }

        /// <summary>
        /// Gets BPUA service
        /// </summary>
        /// <param name="bpuaServicekey">BPUA service key</param>
        /// <returns>BPUA service</returns>
        public static async Task<IBpuaService?> GetBpuaServiceAsync(string? bpuaServicekey)
        {
            if (string.IsNullOrEmpty(bpuaServicekey))
            {
                return null;
            }

            IBpuaApplication bppApplication = BpuaApplication.GetInstance();

            bpuaServicekey = bpuaServicekey.Trim('/');
            IBpuIdentifier bpuIdentifier = new BpuIdentifier(bpuaServicekey);
            UseCaseActivationResult useCaseActivationResult = await ((BpuaApplication)bppApplication).ActivateUseCaseAsync(bpuIdentifier);
            if (useCaseActivationResult.Succeeded)
            {
                return bppApplication.GetRequestHandler(bpuaServicekey);
            }

            return null;
        }
        #endregion
    }
}
