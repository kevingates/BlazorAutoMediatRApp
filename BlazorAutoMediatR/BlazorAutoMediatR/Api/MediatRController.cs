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

        private static readonly Dictionary<string, Type> _requestTypes;
        private readonly IMediator _mediator;

        static MediatRController()
        {
            _requestTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>)))
                .ToDictionary(t => t.FullName!.Replace(".","_"), t => t);
        }

        public MediatRController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{requestName}")]
        public async Task<IActionResult> HandleRequest(string requestName, [FromBody] object request)
        {

            if (!_requestTypes.TryGetValue(requestName, out var requestType))
            {
                return NotFound("Unknown request type.");
            }
            if(request is null)
            {
                return BadRequest("Request object is null.");
            }

             
            var requestInstance = JsonSerializer.Deserialize(request.ToString()!, requestType);
            if (requestInstance is null)
            {
                return Problem("Failed to deserialize request object.");
            }

            var validationContext = new ValidationContext(requestInstance);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(requestInstance, validationContext, validationResults, true);

            if (!isValid)
            {
                return BadRequest(validationResults.Select(r => r.ErrorMessage));
            }

            var response = await _mediator.Send(requestInstance);
            return Ok(response);
        }
    }

}
