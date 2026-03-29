using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using BPUA.Http.Contracts;

namespace BPUA.ApplicationServer.Runtime
{
    /// <summary>
    /// Provides receiver controller starter functionality
    /// </summary>
    public class ApplicationStarter
    {
        #region Static Methods
        /// <summary>
        /// Starts receiver controller
        /// </summary>
        /// <param name="webApplication">Web application</param>
        public static void Start(WebApplication webApplication)
        {
            IReceiverController receiverController = webApplication.Services.GetRequiredService<IReceiverController>();
            receiverController.Dispose();
            if (receiverController.ApplicationInitialized)
            {
                webApplication.Run(receiverController.HostUrl);
            }
            else
            {
                Console.WriteLine();
                Console.Read();
            }
        }
        #endregion
    }
}
