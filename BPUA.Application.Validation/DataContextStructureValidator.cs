using BPUA.Application.Contracts;
using BPUA.Application.Validation.Contracts;
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
        /// <returns>The validation result.</returns>
        public IValidationResult Validate(IDataSet? dataContext, ITransitionDataContract transitionDataContract)
        {
            ValidationResult validationResult = new ValidationResult();
            if (dataContext == null)
            {
                return validationResult;
            }

            DataTableStructureValidator transitionDataTableContractValidator = new DataTableStructureValidator();
            foreach (ITransitionDataTableContract transitionDataTableContract in transitionDataContract)
            {
                IDataTable? dataTable = dataContext[transitionDataTableContract.TableName];
                if (dataTable == null)
                {
                    if (transitionDataTableContract.IsRequired)
                    {
                        validationResult.AddIssue(new ValidationIssue("RequiredTableIsMissing", "Required table is missing.", ValidationIssueSeverity.Error, transitionDataTableContract.TableName));
                    }
                }
                else
                {
                    transitionDataTableContractValidator.Validate(transitionDataTableContract, dataTable, validationResult); 
                }
            }

            return validationResult;
        }
        #endregion
    }
}
