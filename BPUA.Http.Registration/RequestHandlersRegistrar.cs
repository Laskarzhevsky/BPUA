using System;

using SkySoft.ApplicationServer.Configuration;
using SkySoft.BPPCore;
using BPUA.Http.Drivers;

namespace BPUA.Http.Registration
{
    /// <summary>
    /// Provides request handlers registrar functionality
    /// </summary>
    class RequestHandlersRegistrar
    {
        #region Static Methods
        /// <summary>
        /// Register request handlers
        /// </summary>
        /// <param name="handlersRegistry">Handlers registry</param>
        public static void Register(IHandlersRegistry handlersRegistry)
        {
            RegisterTransceiverController(handlersRegistry);
        }
        #endregion

        #region Private Static Methods
        /// <summary>
        /// Registers TransceiverController request handler
        /// </summary>
        /// <param name="handlersRegistry">Handlers registry</param>
        static void RegisterTransceiverController(IHandlersRegistry handlersRegistry)
        {
            Type type = typeof(TransceiverController);
            string key = NameCaclulator.GetRequestHandlerFullName(
                TransceiverController.DomainName,
                TransceiverController.UseCaseName,
                TransceiverController.ApplicationLayerName,
                TransceiverController.StateName,
                TransceiverController.TransitionName);
            handlersRegistry.Add(key, type);
        }
        #endregion
    }
}
