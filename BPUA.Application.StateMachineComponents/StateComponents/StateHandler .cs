using BPUA.Application.CommonComponents;
using BPUA.Application.Contracts;
using BPUA.Core;

using PocoDataSet.Extensions;
using PocoDataSet.IData;

using System.Threading.Tasks;

namespace BPUA.Application.StateMachineComponents
{
    /// <summary>
    /// Provides state handler functionality
    /// </summary>
    public abstract class StateHandler : RequestHandler, IStateHandler
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public StateHandler() : base()
        {
        }

        /// <summary>
        /// Creates an instance, taking the state handler identity as arguments
        /// </summary>
        /// <param name="domainName">Domain name</param>
        /// <param name="useCaseName">Use case name</param>
        /// <param name="applicationLayerName">Application layer name</param>
        /// <param name="stateName">State name</param>
        public StateHandler(string domainName, string useCaseName, string applicationLayerName, string stateName) : base(domainName, useCaseName, applicationLayerName, stateName)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets component identifier
        /// </summary>
        public override string ComponentIdentifier
        {
            get
            {
                return KeyCompiler.CompileStateHandlerKey(BpuaIdentifier.DomainName, BpuaIdentifier.UseCaseName, BpuaIdentifier.ApplicationLayerName, BpuaIdentifier.StateName);
            }
        }
        #endregion

        /// <summary>
        /// Handles request
        /// </summary>
        /// <param name="bpuaIdentifier">BPUA identifier</param>
        /// <returns>Response transition context</returns>
        public async Task<IDataSet?> HandleRequestAsync(IBPUAIdentifier bpuaIdentifier)
        {
            IDataSet dataSet = DataSetFactory.CreateDataSet();

            return await HandleRequestAsync(dataSet);
        }
    }
}
