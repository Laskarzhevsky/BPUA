using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

//using SkySoft.OS.INT;

namespace BPUA.Application.Registration
{
    /// <summary>
    /// Provides BPP application components registrar functionality
    /// </summary>
    class ApplicationComponentsRegistrar
    {
        #region Static Methods
        /// <summary>
        /// Registers BPP application components
        /// </summary>
        /// <param name="webApplicationBuilder">Web application builder</param>
        public static void Register(WebApplicationBuilder webApplicationBuilder)
        {
/*
            webApplicationBuilder.Services.AddSingleton<IApplicationCache, SkySoft.OS.ApplicationCache>();
            webApplicationBuilder.Services.AddTransient<IOS, SkySoft.OS.OS>();
*/
        }
        #endregion
    }
}
