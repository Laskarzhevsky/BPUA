using BPUA.Application.Contracts;
using BPUA.Application.Validation.Contracts;
using PocoDataSet.IData;

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
        /// <param name="transitionDataTableContract">The transition data table contract.</param>
        /// <param name="dataTable">The data table to validate.</param>
        /// <param name="validationResult">The validation result.</param>
        public void Validate(ITransitionDataTableContract transitionDataTableContract, IDataTable dataTable, ValidationResult validationResult)
        {
            ValidateMinimumRowsCount(transitionDataTableContract, dataTable, validationResult);
            ValidateMaximumRowsCount(transitionDataTableContract, dataTable, validationResult);
        }

        /// <summary>
        /// Validates maximum rows count.
        /// </summary>
        /// <param name="transitionDataTableContract">The transition data table contract.</param>
        /// <param name="dataTable">The data table to validate.</param>
        /// <param name="validationResult">The validation result.</param>
        protected virtual void ValidateMaximumRowsCount(ITransitionDataTableContract transitionDataTableContract, IDataTable dataTable, ValidationResult validationResult)
        {
            if (transitionDataTableContract.MaximumRowsCount == null)
            {
                return;
            }

            if (dataTable.Rows.Count > transitionDataTableContract.MaximumRowsCount.Value)
            {
                validationResult.AddIssue(new ValidationIssue("MaximumRowsCountExceeded", "Table contains more rows than allowed by contract.", ValidationIssueSeverity.Error, transitionDataTableContract.TableName));
            }
        }

        /// <summary>
        /// Validates minimum rows count.
        /// </summary>
        /// <param name="transitionDataTableContract">The transition data table contract.</param>
        /// <param name="dataTable">The data table to validate.</param>
        /// <param name="validationResult">The validation result.</param>
        protected virtual void ValidateMinimumRowsCount(ITransitionDataTableContract transitionDataTableContract, IDataTable dataTable, ValidationResult validationResult)
        {
            if (dataTable.Rows.Count < transitionDataTableContract.MinimumRowsCount)
            {
                validationResult.AddIssue(new ValidationIssue( "MinimumRowsCountNotReached", "Table contains fewer rows than required by contract.", ValidationIssueSeverity.Error, transitionDataTableContract.TableName));
            }
        }
        #endregion
    }
}
