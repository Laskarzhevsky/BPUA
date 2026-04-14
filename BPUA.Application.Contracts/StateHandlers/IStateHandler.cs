using BPUA.Core;

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
        /// <param name="bpuaIdentifier">BPUA identifier</param>
        /// <returns>Response transition context</returns>
        Task<IDataSet?> HandleRequestAsync(IBPUAIdentifier bpuaIdentifier);
    }
}
