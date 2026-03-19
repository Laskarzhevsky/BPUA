using System;
using System.Threading.Tasks;

namespace BPUA.Application.ApplicationUseCaseLogic
{
    public class ApplicationUseCaseTransitionHandler
    {
        #region Event handlers
        /// <summary>
        /// Handles RequestHandler.RequestService event
        /// IBPUAApplication interface implementaion
        /// </summary>
        /// <param name="eventSource">Event source</param>
        /// <param name="args">Event arguments</param>
        public async Task RequestHandler_RequestServiceEvent(object? eventSource, EventArgs args)
        {
        }
        #endregion
    }
}
