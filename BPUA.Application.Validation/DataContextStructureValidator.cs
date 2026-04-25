using BPUA.Application.Contracts;
using BPUA.Core;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.IData;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Provides validation of data set against transition data contract.
    /// </summary>
    public class DataContextStructureValidator : IDataContextStructureValidator
    {
        #region Public Methods
        /// <summary>
        /// Validates data context against transition data contract.
        /// </summary>
        /// <param name="dataContext">The data context to validate.</param>
        /// <param name="transitionDataContract">The transition data contract.</param>
        public void Validate(IDataSet? dataContext, ITransitionDataContract transitionDataContract)
        {
            if (dataContext == null)
            {
                return;
            }

            IBPUAIdentifier? bpuaIdentifier = dataContext.GetBpuaIdentifier();
            if (bpuaIdentifier == null)
            {
                dataContext.AddMessage(MessageType.Error, "Required BPUA identifier metadata is missing.");
                return;
            }

            DataTableStructureValidator transitionDataTableContractValidator = new DataTableStructureValidator();
            foreach (ITransitionDataTableContract transitionDataTableContract in transitionDataContract)
            {
                IDataTable? dataTable = null;
                dataContext.TryGetTable(transitionDataTableContract.TableName, out dataTable);
                if (dataTable == null)
                {
                    if (transitionDataTableContract.IsRequired)
                    {
                        dataContext.AddMessage(MessageType.Error, "Required table is missing", transitionDataTableContract.TableName, bpuaIdentifier.ApplicationLayerName);
                    }
                }
                else
                {
                    transitionDataTableContractValidator.Validate(dataContext, bpuaIdentifier, transitionDataTableContract, dataTable); 
                }
            }
        }
        #endregion
    }
}
