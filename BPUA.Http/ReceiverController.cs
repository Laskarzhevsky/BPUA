using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using SkySoft.Communication;
using SkySoft.Contracts;
using BPUA.Http.Contracts;
using SkySoft.ICommunication;
using SkySoft.OS.INT;

namespace BPUA.Http
{
    /// <summary>
    /// Provides receiver controller functionality
    /// </summary>
    [ApiController]
    public partial class ReceiverController : ControllerBase, IReceiverController
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="operatingSystem">Operating system</param>
        public ReceiverController(IOS operatingSystem)
        {
            OperatingSystem = operatingSystem;
            ApplicationInitialized = OperatingSystem.ApplicationsInitialized;
            HostUrl = OperatingSystem.Logger.ApplicationLayerUrl;
        }
        #endregion

        #region Public Methods
        [HttpGet]
        [Route("online")]
        /// <summary>
        /// Verifies API online status
        /// </summary>
        /// <returns>Health check result</returns>
        public string VerifyApiOnlineStatus()
        {
            return "OK";
        }

        [HttpPost]
        [Route("processrequest")]
        /// <summary>
        /// Processes request
        /// </summary>
        /// <returns>Result of processed request</returns>
        public async Task<IActionResult> ProcessRequestAsync()
        {
            string? plainText = null;
            using (var reader = new StreamReader(Request.Body))
            {
                plainText = await reader.ReadToEndAsync();
            }

            IDataContainer? dataContainer = DataContainer.Deserialize(plainText);
            if (dataContainer == null)
            {
                return BadRequest();
            }
            else
            {
                DateTime start = DateTime.Now;
                dataContainer = await OperatingSystem.RedirectRequestToRequestHandlerAsync(dataContainer);
                if (dataContainer.TraceStatistics)
                {
                    DateTime end = DateTime.Now;
                    TimeSpan elapsed = end - start;
                    string formattedMessage = $"BPUA.Http.ReceiverController: The request was processed within {elapsed.TotalMilliseconds} milliseconds";
                    dataContainer.SetMessage(formattedMessage, MessageType.Trace, OperatingSystem!.Logger.ApplicationLayerFullName, OperatingSystem.Logger.ApplicationLayerUrl);
                }

                // Serialize data container
                string serializedDataContainer = DataContainer.Serialize(dataContainer);
                return Ok(serializedDataContainer);
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets flag indicating whether application initialized
        /// IReceiverController interface implementation
        /// </summary>
        public bool ApplicationInitialized
        {
            get;
        }

        /// <summary>
        /// Gets host URL
        /// IReceiverController interface implementation
        /// </summary>
        public string? HostUrl
        {
            get; private set;
        }
        #endregion

        #region Private Properties
        /// <summary>
        /// Gets or sets operating system
        /// </summary>
        IOS OperatingSystem
        {
            get; set;
        }
        #endregion
    }
}
