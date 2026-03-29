using System.Threading.Tasks;

using SkySoft.BPPApplication;
using SkySoft.BPPCore;
using SkySoft.Communication;
using SkySoft.Contracts;
using SkySoft.IBPPApplication;
using SkySoft.ICommunication;

namespace BPUA.Http.Drivers
{
    /// <summary>
    /// Provides transceiver controller functionality
    /// </summary>
    public partial class TransceiverController : SkySoft.BPPApplication.RequestHandler
    {
        #region Identification
        public const string DomainName = SkySoft.Contracts.DomainNames.SKYSOFT;
        public const string UseCaseName = SkySoft.Contracts.UseCaseTypes.CONTROLLER;
        public const string ApplicationLayerName = SkySoft.Contracts.ApplicationLayerNames.NFA;
        public const string StateName = default!;
        public const string TransitionName = SkySoft.Contracts.TransitionTypes.SENDING_REQUEST_TO_REMOTE_SERVER;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public TransceiverController() : base(DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName)
        {
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Adds request data container validation rules
        /// </summary>
        protected override void AddRequestDataContainerValidationRules()
        {
            RequestDataContainerValidationRules.Add(new SkipDataContainerValidation(Direction.Request, TypeFullName));
        }

        /// <summary>
        /// Adds response container validation rules
        /// </summary>
        protected override void AddResponseDataContainerValidationRules()
        {
            ResponseDataContainerValidationRules.Add(new SkipDataContainerValidation(Direction.Response, TypeFullName));
        }

        /// <summary>
        /// Handles request aynchronously
        /// </summary>
        protected override async Task HandleRequestAsync()
        {
            await GetRemoteServerDnsRecordFromDnsClientAsync();
            ValidateRemoteServerDnsRecord();
            if (DnsRecordOfRemoteServerValid)
            {
                await SendRequestToRemoteServerAsync();
            }
            else
            {
                await GetDnsServerDnsRecordFromDnsClientAsync();
                ValidateDnsServerDnsRecord();
                if (DnsRecordOfDnsServerValid)
                {
                    await SendRequestToDnsServerToSearchRemoteServerDnsRecordAsync();
                    ValidateRemoteServerDnsRecord();
                    if (DnsRecordOfRemoteServerValid)
                    {
                        await SaveRemoteServerDnsRecordWithDnsClientAsync();
                        await SendRequestToRemoteServerAsync();
                    }
                }
                else
                {
                    IDataCollection<RequestMetadataDTO>? requestMetadataDataCollection = DataContainer.GetDataColletion<RequestMetadataDTO>(SkySoft.Contracts.DataCollectionTypes.REQUEST_METADATA);
                    IRequestMetadataDTO requestMetadataDTO = requestMetadataDataCollection![requestMetadataDataCollection.Count - 2];
                    string requestHandlerFullName = NameCaclulator.GetRequestHandlerFullName(
                        requestMetadataDTO.DomainName,
                        requestMetadataDTO.UseCaseName,
                        requestMetadataDTO.ApplicationLayerName,
                        requestMetadataDTO.StateName,
                        requestMetadataDTO.TransitionName);
                    DataContainer.SetMessage($"Cannot find {requestHandlerFullName} request handler", MessageType.Error, ApplicationLayerFullName, ApplicationLayerUrl);
                }
            }
        }

        /// <summary>
        /// Releases resources
        /// </summary>
        public override void ReleaseResources()
        {
            RemoteServerDnsRecord = null;
            RequestMetadataDTO = null;
            base.ReleaseResources();
        }
        #endregion
    }
}
