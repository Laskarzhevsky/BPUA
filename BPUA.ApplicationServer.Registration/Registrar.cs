using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using BPUA.ApplicationServer.Configuration;

namespace BPUA.ApplicationServer.Registration
{
    /// <summary>
    /// Provides registrar functionality
    /// </summary>
    public class Registrar
    {
        #region Static Methods
        /// <summary>
        /// Register application initializer
        /// </summary>
        /// <param name="webApplicationBuilder">Web application builder</param>
        public static void Register(WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddSingleton<BPUA.ApplicationServer.Configuration.IInitializersRegistry, BPUA.ApplicationServer.Configuration.InitializersRegistry>();
            webApplicationBuilder.Services.AddSingleton<BPUA.ApplicationServer.Configuration.IHandlersRegistry, BPUA.ApplicationServer.Configuration.HandlersRegistry>();

            BPUA.Application.Registration.Registrar.Register(webApplicationBuilder);
            BPUA.Http.Registration.Registrar.Register(webApplicationBuilder);
            SkySoft.Logging.REG.Registrar.Register(webApplicationBuilder);
        }

        /// <summary>
        /// Registers components
        /// </summary>
        /// <param name="handlersRegistry">Handlers registry</param>
        public static void RegisterComponents(IHandlersRegistry handlersRegistry)
        {
            BPUA.Application.Registration.Registrar.RegisterComponents(handlersRegistry);
            BPUA.Http.Registration.Registrar.RegisterComponents(handlersRegistry);
        }
        #endregion
    }
}
