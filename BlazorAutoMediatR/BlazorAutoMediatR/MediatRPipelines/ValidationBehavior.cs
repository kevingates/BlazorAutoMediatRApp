using BlazorAutoMediatR.MediatRPipelines.CustomExceptions;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BlazorAutoMediatR.MediatRPipelines
{
	public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			var context = new ValidationContext(request);
			var validationResults = new List<ValidationResult>();
			var isValid = Validator.TryValidateObject(request, context, validationResults, true);

			if (!isValid)
			{
				throw new ModelValidationException(validationResults);
			}

			return await next();
		}
	}
}
