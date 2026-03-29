using System;
using System.Net.Http;
using System.Threading.Tasks;

using SkySoft.Communication;
using SkySoft.ICommunication;

namespace BPUA.Http
{
    /// <summary>
    /// Provides transceiver functionality
    /// </summary>
    public class Transceiver
    {
        #region Public Methods
        /// <summary>
        /// Transceives data container
        /// </summary>
        /// <param name="requestDataContainer">Request data container</param>
        /// <param name="baseAddress">Base address</param>
        /// <param name="subAddress">Sub address</param>
        /// <param name="timeout">Sets how long a connection can be in the pool to be considered reusable in milliseconds (by default - infinite)</param>
        /// <returns>Response data container if request was successful, otherwise NULL</returns>
        public async Task<IDataContainer?> TransceiveDataContainerAsync(IDataContainer requestDataContainer, string baseAddress, string subAddress, int? timeout)
        {
            HttpClient httpClient = default!;
            if (timeout.HasValue)
            {
                SocketsHttpHandler handler = new SocketsHttpHandler();
                handler.PooledConnectionLifetime = TimeSpan.FromMilliseconds(timeout.Value);
                httpClient = new HttpClient(handler, false);
            }
            else
            {
                httpClient = new HttpClient();
            }

            IDataContainer? responseDataContainer = await TransceiveDataContainerAsync(httpClient, requestDataContainer, baseAddress, subAddress);
            return responseDataContainer;
        }

        /// <summary>
        /// Transceives data container
        /// </summary>
        /// <param name="httpClient">Http client</param>
        /// <param name="requestDataContainer">Request data container</param>
        /// <param name="baseAddress">Base address</param>
        /// <param name="timeout">Sets how long a connection can be in the pool to be considered reusable in milliseconds (by default - infinite)</param>
        /// <returns>Response data container if request was successful, otherwise NULL</returns>
        public async Task<IDataContainer?> TransceiveDataContainerAsync(HttpClient httpClient, IDataContainer requestDataContainer, string baseAddress, string subAddress)
        {
            IDataContainer? responseDataContainer = null;
            try
            {
                string requestSerializedDataContainer = DataContainer.Serialize(requestDataContainer);
                StringContent stringContent = new StringContent(requestSerializedDataContainer, System.Text.Encoding.ASCII, "text/plain");

                httpClient.BaseAddress = new Uri(baseAddress);
                var response = await httpClient.PostAsync(subAddress, stringContent);

                var responseSerializedDataContainer = await response.Content.ReadAsStringAsync();
                responseDataContainer = DataContainer.Deserialize(responseSerializedDataContainer);
            }
            catch (HttpRequestException httpRequestException)
            {
                responseDataContainer = requestDataContainer;
                responseDataContainer.Exception = httpRequestException;
            }

            return responseDataContainer;
        }
        #endregion
    }
}
