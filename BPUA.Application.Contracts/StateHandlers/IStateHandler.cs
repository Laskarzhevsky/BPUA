using PocoDataSet.IData;

using System.Threading.Tasks;

namespace BPUA.Application.Contracts
{
    /// <summary>
    /// Defines state handler functionality
    /// </summary>
    public interface IStateHandler : IRequestHandler
    {
        /// <summary>
        /// Handles request
        /// </summary>
        /// <returns>Response transition context</returns>
        Task<IDataSet?> HandleRequestAsync();
    }
}
