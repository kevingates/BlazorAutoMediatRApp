using Domain.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BlazorAutoMediatR.Api
{
	[ApiController]
	[Route("api/[controller]")]
	public class MediatorController : ControllerBase
	{
		private readonly IMediatorService _mediatorService;

		public MediatorController(IMediatorService mediatorService)
		{
			_mediatorService = mediatorService;
		}

		[HttpPost]
		public async Task<IActionResult> PostAsync([FromBody] ApiRequest request)
		{
			try
			{
				var requestType = Type.GetType(request.TypeFullName);
				var method = _mediatorService.GetType().GetMethod("Send");
				var genericMethod = method.MakeGenericMethod(requestType);
				var typedItem = Convert.ChangeType(request.Request, requestType);

				if (typedItem is IValidatableObject validatableRequest)
				{
					var results = validatableRequest.Validate(new ValidationContext(validatableRequest));
					if (results.Any())
					{
						return BadRequest(results.Select(r => r.ErrorMessage));
					}
				}

				var response = await (Task<object>)genericMethod.Invoke(_mediatorService, new object[] { typedItem });
				return Ok(response);
			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
		}
	}
}
