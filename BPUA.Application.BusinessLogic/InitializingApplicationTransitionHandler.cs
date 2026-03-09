using System.Threading.Tasks;

using BPUA.Application.Contracts;
using BPUA.Application.EventArguments;
using BPUA.Application.RequestHandlers;
using BPUA.Core;
using PocoDataSet.BPUAExtensions;

using PocoDataSet.Extensions;
using PocoDataSet.IData;

namespace BPUA.Application.BusinessLogic
{
    [RegisterAsBPUAService]
    public class InitializingApplicationTransitionHandler : BusinessLogicTransitionHandler, IBusinessLogicTransitionHandler
    {
        #region Identification
        public static string DomainName = BPUA.Application.Contracts.DomainNames.BPUA;
        public static string UseCaseName = BPUA.Application.Contracts.UseCaseNames.APPLICATION;
        public static string ApplicationLayerName = BPUA.Application.Contracts.ApplicationLayersNames.BL;
        public static string StateName = default!;
        public static string TransitionName = BPUA.Application.Contracts.TransitionsNames.INITIALIZING_APPLICATION;

        /// <summary>
        /// Gets service key
        /// </summary>
        public static string ServiceKey
        {
            get
            {
                return KeyCompiler.CompileTransitionHandlerKey(DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public InitializingApplicationTransitionHandler() : base(DomainName, UseCaseName, ApplicationLayerName, StateName, TransitionName)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Handles request
        /// </summary>
        /// <param name="requestDataSet">Request data set</param>
        /// <returns>Response data set</returns>
        public override async Task<IDataSet?> HandleRequestAsync(IDataSet? requestDataSet)
        {
            RequestToNextLayerEventArgs requestToNextLayerEventArgs = new RequestToNextLayerEventArgs(requestDataSet);
            await RaiseServiceRequestEventAsync(requestToNextLayerEventArgs);

            IDataSet? responseDataSet = requestToNextLayerEventArgs.DataSet;
            IDataTable transitionMetadataTable = responseDataSet!.GetTransitionMetadataTable();
            for (int i = 0; i < transitionMetadataTable.Rows.Count; i++)
            {
                ITransitionMetadata transitionMetadata = DataRowExtensions.AsInterface<ITransitionMetadata>(transitionMetadataTable.Rows[i]);
                transitionMetadata.Available = true;
            }

            return responseDataSet;
        }
        #endregion
    }
}
