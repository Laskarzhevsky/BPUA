using BPUA.Application.Contracts;
using BPUA.Application.Validation.Contracts;
using PocoDataSet.IData;
using System.Collections.Generic;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Provides validation of data set against transition data contract.
    /// </summary>
    public class TransitionDataContractValidator : ITransitionDataContractValidator
    {
        #region Public Methods
        /// <summary>
        /// Validates data set against transition data contract.
        /// </summary>
        /// <param name="transitionDataContract">The transition data contract.</param>
        /// <param name="dataSet">The data set to validate.</param>
        /// <returns>The validation result.</returns>
        public IValidationResult Validate(ITransitionDataContract transitionDataContract, IDataSet? dataSet)
        {
            ValidationResultBuilder validationResultBuilder = new ValidationResultBuilder();
            if (dataSet == null)
            {
                return validationResultBuilder.Build();
            }

            IReadOnlyList<ITransitionDataTableContract> transitionDataTableContracts = transitionDataContract.DataTableContracts;
            for (int i = 0; i < transitionDataTableContracts.Count; i++)
            {
                ITransitionDataTableContract transitionDataTableContract = transitionDataTableContracts[i];
                IDataTable? dataTable = dataSet[transitionDataTableContracts[i].TableName];
                if (dataTable == null)
                {
                    if (transitionDataTableContract.IsRequired)
                    {
                        validationResultBuilder.AddIssue(new ValidationIssue("RequiredTableIsMissing", "Required table is missing.", ValidationIssueSeverity.Error, transitionDataTableContract.TableName));
                    }
                }
                else
                {
                    TransitionDataTableContractValidator transitionDataTableContractValidator = new TransitionDataTableContractValidator();
                    transitionDataTableContractValidator.Validate(transitionDataTableContract, dataTable, validationResultBuilder); 
                }
            }

            return validationResultBuilder.Build();
        }
        #endregion
    }
}
