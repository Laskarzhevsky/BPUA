using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using SkySoft.OS.INT;

namespace BPUA.Http.Registration
{
    /// <summary>
    /// Provides drivers registrar functionality
    /// </summary>
    class DriversRegistrar
    {
        #region Static Methods
        /// <summary>
        /// Register request handlers
        /// </summary>
        /// <param name="webApplicationBuilder">Web application builder</param>
        public static void Register(WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddTransient<IDriver, BPUA.Http.Drivers.TransceiverDriver>();
        }
        #endregion
    }
}
