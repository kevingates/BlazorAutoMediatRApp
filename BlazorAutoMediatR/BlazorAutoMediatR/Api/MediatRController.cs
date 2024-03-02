using BlazorAutoMediatR.MediatRPipelines.CustomExceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace BlazorAutoMediatR.Api
{
    [ApiController]
	[Route("api/[controller]")]
	public class MediatRController : ControllerBase
	{
		private static readonly Dictionary<string, (Type Type, bool IsRequestOfT)> _requestTypes;
		private readonly IMediator _mediator;

		static MediatRController()
		{
			_requestTypes = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.Where(t => t.GetInterfaces().Any(i => i == typeof(IRequest) || (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>))))
				.ToDictionary(
					t => t.FullName!.Replace(".", "_"),
					t => (
						Type: t,
						IsRequestOfT: t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>))
					)
				);
		}

		public MediatRController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("{requestName}")]
		public async Task<IActionResult> HandleRequest(string requestName, [FromBody] JsonElement requestJson)
		{
			try
			{
				if (!_requestTypes.TryGetValue(requestName, out var requestTypeInfo))
				{
					return NotFound("Unknown request type.");
				}

				var (requestType, isRequestOfT) = requestTypeInfo;

				var requestJsonString = requestJson.GetRawText();
				var options = new JsonSerializerOptions
				{
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				};

				var requestInstance = JsonSerializer.Deserialize(requestJsonString, requestType, options);
				if (requestInstance is null)
				{
					return Problem("Failed to deserialize request object.");
				}


				try
				{
					if (isRequestOfT)
					{
						var response = await _mediator.Send(requestInstance);
						return Ok(response);
					}
					else
					{
						await _mediator.Send((IRequest)requestInstance);
						return Ok();
					}
				}
				catch (AuthorizationException ex)
				{
					// Can't use Forbid unless authentication scheme is established:
					// https://stackoverflow.com/questions/44597099/asp-net-core-giving-me-code-500-on-forbid
					return StatusCode(403, ex.Message);
                }
				catch (ModelValidationException ex)
				{
					return BadRequest(ex.ValidationResults);
				}
				catch (Exception ex)
				{
					return Problem(ex.Message);
				}
			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
		}
	}
}