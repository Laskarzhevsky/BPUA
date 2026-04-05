using System.Threading.Tasks;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines state handler functionality
    /// </summary>
    public interface IStateHandler : IRequestHandler
    {
        #region Methods
        /// <summary>
        /// Initializes component
        /// </summary>
        Task InitializeComponent();
        #endregion
    }
}
