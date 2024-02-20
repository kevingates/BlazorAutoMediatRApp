using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BlazorAutoMediatR.Api
{
	[ApiController]
	[Route("api/[controller]")]
	public class MediatRController<TRequest, TResponse> : ControllerBase
	where TRequest : IRequest<TResponse>
	{
		private readonly IMediator _mediator;
		private readonly ILogger<MediatRController<TRequest, TResponse>> _logger;

		public MediatRController(IMediator mediator, ILogger<MediatRController<TRequest, TResponse>> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}

		[HttpPost]
		public async Task<IActionResult> PostAsync([FromBody] TRequest request)
		{
			try
			{
				// Validate request using data annotations
				var validationContext = new ValidationContext(request);
				var validationResults = new List<ValidationResult>();
				if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
				{
					return BadRequest(validationResults.Select(v => v.ErrorMessage));
				}

				var response = await _mediator.Send(request);
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while processing the request.");
				return Problem("An error occurred while processing the request.", statusCode: 500);
			}
		}
	}

}
