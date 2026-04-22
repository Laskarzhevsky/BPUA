using BPUA.Application.Contracts;
using BPUA.Application.Validation.Contracts;
using PocoDataSet.IData;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Provides validation of transition data contract against provided data set.
    /// </summary>
    internal class TransitionDataTableContractValidator
    {
        #region Public Methods
        /// <summary>
        /// Validates single table contract.
        /// </summary>
        /// <param name="transitionDataTableContract">The transition data table contract.</param>
        /// <param name="dataTable">The data table to validate.</param>
        /// <param name="validationResultBuilder">The validation result builder.</param>
        public void Validate(ITransitionDataTableContract transitionDataTableContract, IDataTable dataTable, ValidationResultBuilder validationResultBuilder)
        {
            ValidateMinimumRowsCount(transitionDataTableContract, dataTable, validationResultBuilder);
            ValidateMaximumRowsCount(transitionDataTableContract, dataTable, validationResultBuilder);
        }

        /// <summary>
        /// Validates maximum rows count.
        /// </summary>
        /// <param name="transitionDataTableContract">The transition data table contract.</param>
        /// <param name="dataTable">The data table to validate.</param>
        /// <param name="validationResultBuilder">The validation result builder.</param>
        protected virtual void ValidateMaximumRowsCount(ITransitionDataTableContract transitionDataTableContract, IDataTable dataTable, ValidationResultBuilder validationResultBuilder)
        {
            if (transitionDataTableContract.MaximumRowsCount == null)
            {
                return;
            }

            if (dataTable.Rows.Count > transitionDataTableContract.MaximumRowsCount.Value)
            {
                validationResultBuilder.AddIssue(new ValidationIssue("MaximumRowsCountExceeded", "Table contains more rows than allowed by contract.", ValidationIssueSeverity.Error, transitionDataTableContract.TableName));
            }
        }

        /// <summary>
        /// Validates minimum rows count.
        /// </summary>
        /// <param name="transitionDataTableContract">The transition data table contract.</param>
        /// <param name="dataTable">The data table to validate.</param>
        /// <param name="validationResultBuilder">The validation result builder.</param>
        protected virtual void ValidateMinimumRowsCount(ITransitionDataTableContract transitionDataTableContract, IDataTable dataTable, ValidationResultBuilder validationResultBuilder)
        {
            if (dataTable.Rows.Count < transitionDataTableContract.MinimumRowsCount)
            {
                validationResultBuilder.AddIssue(new ValidationIssue( "MinimumRowsCountNotReached", "Table contains fewer rows than required by contract.", ValidationIssueSeverity.Error, transitionDataTableContract.TableName));
            }
        }
        #endregion
    }
}
