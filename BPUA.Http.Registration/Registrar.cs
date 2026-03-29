using Microsoft.AspNetCore.Builder;

using SkySoft.ApplicationServer.Configuration;

namespace BPUA.Http.Registration
{
    /// <summary>
    /// Provides registrar functionality
    /// </summary>
    public class Registrar
    {
        #region Static Methods
        /// <summary>
        /// Registers components
        /// </summary>
        /// <param name="webApplicationBuilder">Web application builder</param>
        public static void Register(WebApplicationBuilder webApplicationBuilder)
        {
            DriversRegistrar.Register(webApplicationBuilder);
            ReceiverControllerRegistrar.Register(webApplicationBuilder);
        }

        /// <summary>
        /// Registers components
        /// </summary>
        /// <param name="handlersRegistry">Handlers registry</param>
        public static void RegisterComponents(IHandlersRegistry handlersRegistry)
        {
            RequestHandlersRegistrar.Register(handlersRegistry);
        }
        #endregion
    }
}
