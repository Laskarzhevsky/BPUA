using BPUA.InfrastructureServer.Contracts;

using System.Threading.Tasks;

namespace BPUA.InfrastructureServer.DPL
{
    /// <summary>
    /// RegisteringHost service handler
    /// </summary>
    public partial class RegisteringHostRequestHandler
    {
        #region Private Methods
        /// <summary>
        /// Registers host
        /// </summary>
        async Task RegisteringHost()
        {
            await RaiseServiceRequestEventAsync(RequestNames.REGISTER_HOST);
        }
        #endregion
    }
}
