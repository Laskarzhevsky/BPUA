using System.Threading.Tasks;

using SkySoft.BPPCore;
using SkySoft.Communication;
using SkySoft.Contracts;
using SkySoft.DnsRecord.DTO;
using SkySoft.ICommunication;

namespace BPUA.Http.Drivers
{
    /// <summary>
    /// Provides transceiver controller functionality
    /// </summary>
    public partial class TransceiverController : SkySoft.BPPApplication.RequestHandler
    {
        #region Private Methods
        /// <summary>
        /// Adds DNS record with application layer full name of DNS Server to data container
        /// </summary>
        void AddDnsRecordWithApplicationLayerFullNameOfDnsServerToDataContainer()
        {
            DnsRecordDTO dnsRecordDTO = DataContainer.GetNewDTO<DnsRecordDTO>(SkySoft.Contracts.DataCollectionTypes.DNS_RECORDS);
            dnsRecordDTO.ApplicationLayerFullName = GetApplicationLayerFullName(SkySoft.Contracts.DomainNames.SKYSOFT, null, SkySoft.Contracts.ApplicationLayerNames.DNS_SERVER);
        }

        /// <summary>
        /// Adds DNS record with request handler full name of request to data container
        /// </summary>
        void AddDnsRecordWithRequestHandlerFullNameToDataContainer()
        {
            IDataCollection<RequestMetadataDTO>? requestMetadataDataCollection = DataContainer.GetDataColletion<RequestMetadataDTO>(SkySoft.Contracts.DataCollectionTypes.REQUEST_METADATA);
            IRequestMetadataDTO requestMetadataDTO = requestMetadataDataCollection![requestMetadataDataCollection.Count - 2];
            DnsRecordDTO dnsRecordDTO = DataContainer.GetNewDTO<DnsRecordDTO>(SkySoft.Contracts.DataCollectionTypes.DNS_RECORDS);
            if (requestMetadataDTO.DomainName == SkySoft.Contracts.DomainNames.SKYSOFT)
            {
                dnsRecordDTO.ApplicationLayerFullName = requestMetadataDTO.ApplicationLayerFullName;
            }
            else
            {
                dnsRecordDTO.ApplicationLayerFullName = NameCaclulator.GetRequestHandlerFullName(
                    requestMetadataDTO.DomainName,
                    requestMetadataDTO.UseCaseName,
                    requestMetadataDTO.ApplicationLayerName,
                    requestMetadataDTO.StateName,
                    requestMetadataDTO.TransitionName);
            }
        }

        /// <summary>
        /// Caches last request metadata by removing it from data container
        /// </summary>
        void CacheLastRequestMetadataByRemovingItFromDataContainer()
        {
            RequestMetadataDTO = DataContainer.GetLastDTOByRemovingItFromDataCollection<RequestMetadataDTO>(SkySoft.Contracts.DataCollectionTypes.REQUEST_METADATA);
        }

        /// <summary>
        /// Calculates DNS server URL
        /// </summary>
        void CalculateDnsServerUrl()
        {
            RemoteServerUrl = DnsServerDnsRecord!.Url;
        }

        /// <summary>
        /// Calculates remote server URL
        /// </summary>
        void CalculateRemoteServerUrl()
        {
            RemoteServerUrl = RemoteServerDnsRecord!.Url;
        }

        /// <summary>
        /// Gets DNS server DNS record from data container
        /// </summary>
        void GetDnsServerDnsRecordFromDataContainer()
        {
            DnsServerDnsRecord = DataContainer.GetLastDTOFromDataCollection<DnsRecordDTO>(SkySoft.Contracts.DataCollectionTypes.DNS_RECORDS);
        }

        /// <summary>
        /// Gets remote server DNS record from data container
        /// </summary>
        void GetRemoteServerDnsRecordFromDataContainer()
        {
            RemoteServerDnsRecord = DataContainer.GetLastDTOFromDataCollection<DnsRecordDTO>(SkySoft.Contracts.DataCollectionTypes.DNS_RECORDS);
        }

        /// <summary>
        /// Gets remote server DNS record from DNS client
        /// </summary>
        async Task GetRemoteServerDnsRecordFromDnsClientAsync()
        {
            AddDnsRecordWithRequestHandlerFullNameToDataContainer();

            await RaiseGetRemoteServerDataEventAsync();

            GetRemoteServerDnsRecordFromDataContainer();
        }

        /// <summary>
        /// Gets DNS server DNS record from DNS client
        /// </summary>
        /// <returns></returns>
        async Task GetDnsServerDnsRecordFromDnsClientAsync()
        {
            AddDnsRecordWithApplicationLayerFullNameOfDnsServerToDataContainer();

            await RaiseGetRemoteServerDataEventAsync();

            GetDnsServerDnsRecordFromDataContainer();
        }

        /// <summary>
        /// Raises GetRemoteServerData event
        /// </summary>
        async Task RaiseGetRemoteServerDataEventAsync()
        {
            await RaiseEventAsync(SkySoft.Contracts.TransitionTypes.GETTING_REMOTE_SERVER_DATA);
        }

        /// <summary>
        /// Raises SaveRemoteServerDnsRecordWithDnsClient event
        /// </summary>
        async Task RaiseSaveRemoteServerDnsRecordWithDnsClientEventAsync()
        {
            await RaiseEventAsync(SkySoft.Contracts.TransitionTypes.SAVING_REMOTE_SERVER_DATA);
        }

        /// <summary>
        /// Removes last DNS record from data container
        /// </summary>
        void RemoveLastDnsRecordFromDataContainer()
        {
            DataContainer.RemoveLastDTOFromDataCollection<DnsRecordDTO>(SkySoft.Contracts.DataCollectionTypes.DNS_RECORDS);
        }

        /// <summary>
        /// Restores last request metadata from cache
        /// </summary>
        void RestoreLastRequestMetadataFromCache()
        {
            IDataCollection<RequestMetadataDTO>? requestMetadataDataCollection = DataContainer.GetDataColletion<RequestMetadataDTO>(SkySoft.Contracts.DataCollectionTypes.REQUEST_METADATA);
            requestMetadataDataCollection!.Add(RequestMetadataDTO!);
        }

        /// <summary>
        /// Saves remote server DNS record with DNS client
        /// </summary>
        async Task SaveRemoteServerDnsRecordWithDnsClientAsync()
        {
            await RaiseSaveRemoteServerDnsRecordWithDnsClientEventAsync();
        }

        /// <summary>
        /// Sends request to remote server
        /// </summary>
        /// <returns>Task result</returns>
        async Task SendRequestToRemoteServerAsync()
        {
            CalculateRemoteServerUrl();
            CacheLastRequestMetadataByRemovingItFromDataContainer();

            await TransmitRequestToRemoteServerAsync();

            RestoreLastRequestMetadataFromCache();
        }

        /// <summary>
        /// Sends request to DNS server to search remote server DNS record
        /// </summary>
        /// <returns>Task result</returns>
        async Task SendRequestToDnsServerToSearchRemoteServerDnsRecordAsync()
        {
            DataContainer.RemoveLastDTOFromDataCollection<DnsRecordDTO>(SkySoft.Contracts.DataCollectionTypes.DNS_RECORDS);
            DataContainer.AddRequestMetadata(
                SkySoft.Contracts.DomainNames.SKYSOFT,
                "",
                SkySoft.Contracts.ApplicationLayerNames.DNS_SERVER,
                "",
                SkySoft.Contracts.Constants.SAAS_ + SkySoft.Contracts.TransitionTypes.SEARCHING);

            CalculateDnsServerUrl();
            await TransmitRequestToRemoteServerAsync();
            DataContainer.RemoveCurrentRequestMetadta();

            GetRemoteServerDnsRecordFromDataContainer();
        }

        /// <summary>
        /// Transmits request to remote server
        /// </summary>
        /// <returns>Task result</returns>
        async Task TransmitRequestToRemoteServerAsync()
        {
            BPUA.Http.Transceiver transceiver = new BPUA.Http.Transceiver();
            IDataContainer? responseDataContainer = await transceiver.TransceiveDataContainerAsync(DataContainer, RemoteServerUrl!, "/processrequest", 10000);
            MessageDTO? messageDTO = responseDataContainer!.GetLastDTOFromDataCollection<MessageDTO>(SkySoft.Contracts.DataCollectionTypes.BPP_MESSAGES);
            if (messageDTO != null && messageDTO.Exception != null)
            {
                messageDTO.Message = messageDTO.Exception.Message + " (" + responseDataContainer.ApplicationLayerFullName + ")";
                messageDTO.MessageType = MessageType.Error;
                messageDTO.Exception = null;
            }

            DataContainer = responseDataContainer;
        }

        /// <summary>
        /// Validates DNS server DNS record
        /// </summary>
        void ValidateDnsServerDnsRecord()
        {
            DnsRecordValidator dnsRecordValidator = new DnsRecordValidator(DnsServerDnsRecord!, false);
            dnsRecordValidator.ProcessRequest(DataContainer);
            dnsRecordValidator.ReleaseResources();
            DnsRecordOfDnsServerValid = dnsRecordValidator.DnsRecordDataValid;
        }

        /// <summary>
        /// Validates remote server DNS record
        /// </summary>
        void ValidateRemoteServerDnsRecord()
        {
            DnsRecordValidator dnsRecordValidator = new DnsRecordValidator(RemoteServerDnsRecord!, false);
            dnsRecordValidator.ProcessRequest(DataContainer);
            dnsRecordValidator.ReleaseResources();
            DnsRecordOfRemoteServerValid = dnsRecordValidator.DnsRecordDataValid;
        }
        #endregion

        #region Private Properties
        /// <summary>
        /// Gets or sets DNS server DNS record
        /// </summary>
        DnsRecordDTO? DnsServerDnsRecord
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets remote server DNS record
        /// </summary>
        DnsRecordDTO? RemoteServerDnsRecord
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets flag indicating whether DNS record of remote server valid
        /// </summary>
        bool DnsRecordOfRemoteServerValid
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets flag indicating whether DNS record of DNS server valid
        /// </summary>
        bool DnsRecordOfDnsServerValid
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets remote server URL
        /// </summary>
        string? RemoteServerUrl
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets request metadata
        /// </summary>
        RequestMetadataDTO? RequestMetadataDTO
        {
            get; set;
        }
        #endregion
    }
}
