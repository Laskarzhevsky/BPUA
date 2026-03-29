using Microsoft.AspNetCore.Builder;

using BPUA.ApplicationServer.Configuration;

namespace BPUA.Application.Registration
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
            ApplicationComponentsRegistrar.Register(webApplicationBuilder);
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Registers components
        /// </summary>
        /// <param name="handlersRegistry">Handlers registry</param>
        public static void RegisterComponents(IHandlersRegistry handlersRegistry)
        {
            EventHandlersRegistrar.Register(handlersRegistry);
        }
        #endregion
    }
}
