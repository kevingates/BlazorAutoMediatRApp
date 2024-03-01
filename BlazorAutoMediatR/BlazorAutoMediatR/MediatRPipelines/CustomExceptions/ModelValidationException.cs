using System.ComponentModel.DataAnnotations;

namespace BlazorAutoMediatR.MediatRPipelines.CustomExceptions
{
    public class ModelValidationException : Exception
    {
        public IEnumerable<ValidationResult> ValidationResults { get; }

        public ModelValidationException(IEnumerable<ValidationResult> validationResults)
            : base("Model validation failed")
        {
            ValidationResults = validationResults;
        }
    }
}
