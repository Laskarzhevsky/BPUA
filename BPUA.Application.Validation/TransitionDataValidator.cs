using BPUA.Application.Validation.Contracts;
using PocoDataSet.IData;
using System.Collections.Generic;

namespace BPUA.Application.Validation
{
    /// <summary>
    /// Validator composed from reusable validation rules.
    /// </summary>
    public class TransitionDataValidator : ITransitionDataValidator
    {
        #region Fields
        private readonly IReadOnlyList<IValidationRule> _validationRules;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionDataValidator"/> class.
        /// </summary>
        public TransitionDataValidator(IReadOnlyList<IValidationRule> validationRules)
        {
            _validationRules = validationRules;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Validates transition data set.
        /// </summary>
        /// <param name="dataSet">The data set to validate.</param>
        /// <returns>The validation result.</returns>
        public IValidationResult Validate(IDataSet? dataSet)
        {
            ValidationResultBuilder validationResultBuilder = new ValidationResultBuilder();
            for (int index = 0; index < _validationRules.Count; index++)
            {
                _validationRules[index].Validate(dataSet, validationResultBuilder);
            }

            return validationResultBuilder.Build();
        }
        #endregion
    }
}
