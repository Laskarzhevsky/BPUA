using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using BPUA.Http.Contracts;

namespace BPUA.Http.Registration
{
    /// <summary>
    /// Provides receiver controller registrar functionality
    /// </summary>
    class ReceiverControllerRegistrar
    {
        #region Static Methods
        /// <summary>
        /// Register receiver controller
        /// </summary>
        /// <param name="webApplicationBuilder">Web application builder</param>
        public static void Register(WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddTransient<IReceiverController, ReceiverController>();
        }
        #endregion
    }
}
