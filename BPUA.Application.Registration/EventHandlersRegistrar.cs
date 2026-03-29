using System;

using BPUA.ApplicationServer.Configuration;
//using SkySoft.BPPCore;

namespace BPUA.Application.Registration
{
    /// <summary>
    /// Provides event handlers registrar functionality
    /// </summary>
    class EventHandlersRegistrar
    {
        #region Public Methods
        /// <summary>
        /// Register event handlers
        /// </summary>
        /// <param name="handlersRegistry">Handlers registry</param>
        public static void Register(IHandlersRegistry handlersRegistry)
        {
            RegisterRedirectRequestToNextApplicationLayer(handlersRegistry);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Registers RedirectRequestToNextApplicationLayer event handler
        /// </summary>
        /// <param name="handlersRegistry">Handlers registry</param>
        static void RegisterRedirectRequestToNextApplicationLayer(IHandlersRegistry handlersRegistry)
        {
/*
            Type type = typeof(RedirectRequestToNextApplicationLayerEventHandler);
//            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);
            string key = NameCaclulator.GetRequestHandlerFullName(
                RedirectRequestToNextApplicationLayerEventHandler.DomainName,
                RedirectRequestToNextApplicationLayerEventHandler.UseCaseName,
                RedirectRequestToNextApplicationLayerEventHandler.ApplicationLayerName,
                RedirectRequestToNextApplicationLayerEventHandler.StateName,
                RedirectRequestToNextApplicationLayerEventHandler.EventName);
            handlersRegistry.Add(key, type);
*/
        }
        #endregion
    }
}
