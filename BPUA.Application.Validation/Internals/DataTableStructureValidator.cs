using BPUA.Application.Contracts;
using BPUA.Core;

using PocoDataSet.BpuaExtensions;
using PocoDataSet.IData;

using System;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Provides validation of transition data contract against provided data set.
    /// </summary>
    internal class DataTableStructureValidator
    {
        #region Public Methods
        /// <summary>
        /// Validates single table contract.
        /// </summary>
        /// <param name="dataContext">The data context to validate.</param>
        /// <param name="bpuaIdentifier">The BPUA identifier.</param>
        /// <param name="transitionDataTableContract">The transition data table contract.</param>
        /// <param name="dataTable">The data table to validate.</param>
        public void Validate(IDataSet? dataContext, IBPUAIdentifier bpuaIdentifier, ITransitionDataTableContract transitionDataTableContract, IDataTable dataTable)
        {
            if (transitionDataTableContract.MinimumRowsCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(transitionDataTableContract.MinimumRowsCount));
            }

            if (transitionDataTableContract.MaximumRowsCount != null && transitionDataTableContract.MaximumRowsCount.Value < transitionDataTableContract.MinimumRowsCount)
            {
                throw new ArgumentException("Maximum rows count cannot be less than minimum rows count.", nameof(transitionDataTableContract.MaximumRowsCount));
            }

            ValidateMinimumRowsCount(dataContext, bpuaIdentifier, transitionDataTableContract, dataTable);
            ValidateMaximumRowsCount(dataContext, bpuaIdentifier, transitionDataTableContract, dataTable);
        }

        /// <summary>
        /// Validates maximum rows count.
        /// </summary>
        /// <param name="dataContext">The data context to validate.</param>
        /// <param name="bpuaIdentifier">The BPUA identifier.</param>
        /// <param name="transitionDataTableContract">The transition data table contract.</param>
        /// <param name="dataTable">The data table to validate.</param>
        protected virtual void ValidateMaximumRowsCount(IDataSet? dataContext, IBPUAIdentifier bpuaIdentifier, ITransitionDataTableContract transitionDataTableContract, IDataTable dataTable)
        {
            if (transitionDataTableContract.MaximumRowsCount == null)
            {
                return;
            }

            if (dataTable.Rows.Count > transitionDataTableContract.MaximumRowsCount.Value)
            {
                dataContext.AddMessage(MessageType.Error, "MaximumRowsCountExceeded", $"Table {transitionDataTableContract.TableName} contains more rows than allowed by contract.", bpuaIdentifier.ApplicationLayerName);
            }
        }

        /// <summary>
        /// Validates minimum rows count.
        /// </summary>
        /// <param name="dataContext">The data context to validate.</param>
        /// <param name="bpuaIdentifier">The BPUA identifier.</param>
        /// <param name="transitionDataTableContract">The transition data table contract.</param>
        /// <param name="dataTable">The data table to validate.</param>
        protected virtual void ValidateMinimumRowsCount(IDataSet? dataContext, IBPUAIdentifier bpuaIdentifier, ITransitionDataTableContract transitionDataTableContract, IDataTable dataTable)
        {
            if (dataTable.Rows.Count < transitionDataTableContract.MinimumRowsCount)
            {
                dataContext.AddMessage(MessageType.Error, "MinimumRowsCountNotReached", $"Table {transitionDataTableContract.TableName} contains fewer rows than required by contract.", bpuaIdentifier.ApplicationLayerName);
            }
        }
        #endregion
    }
}
