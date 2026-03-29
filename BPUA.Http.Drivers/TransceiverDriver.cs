using SkySoft.OS;

namespace BPUA.Http.Drivers
{
    /// <summary>
    /// Provides transceiver driver functionality
    /// </summary>
    public class TransceiverDriver : Driver
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public TransceiverDriver()
        {
            ControllerType = SkySoft.Contracts.ControllerTypes.TRANSCEIVER;

            DomainName = SkySoft.Contracts.DomainNames.SKYSOFT;
            UseCaseName = SkySoft.Contracts.UseCaseTypes.CONTROLLER;
            ApplicationLayerName = SkySoft.Contracts.ApplicationLayerNames.NFA;
            StateName = "";
            TransitionName = SkySoft.Contracts.TransitionTypes.SENDING_REQUEST_TO_REMOTE_SERVER;
        }
        #endregion
    }
}
