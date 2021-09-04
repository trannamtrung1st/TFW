using FluentValidation.Results;
using System.Collections.Generic;

namespace TFW.Framework.Validations.Fluent
{
    public interface IValidationResultProvider
    {
        IList<ValidationResult> Results { get; }
    }

    public class DefaultValidationResultProvider : IValidationResultProvider
    {
        protected readonly List<ValidationResult> results;
        public IList<ValidationResult> Results => results;

        public DefaultValidationResultProvider()
        {
            results = new List<ValidationResult>();
        }

    }
}
