using Domain.Core;
using Domain.Core.Queries;
using Domain.Core.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BlazorAutoMediatR.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomainController : ControllerBase
    {
        private readonly IMediatorService _mediator;

        public DomainController(IMediatorService mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("InsertPersonCommand")]
        public async Task<IActionResult> InsertPersonCommand([FromBody] InsertPersonCommand request)
        {
            if (request is IValidatableObject validatableRequest)
            {
                var results = validatableRequest.Validate(new ValidationContext(validatableRequest));
                if (results.Any())
                {
                    return BadRequest(results.Select(r => r.ErrorMessage));
                }
            }

            var result = _mediator.Send(request);

            return Ok(result);
        }

        [HttpPost("GetPersonListQuery")]
        public async Task<IActionResult> GetPersonListQuery([FromBody] GetPersonListQuery request)
        {
            if (request is IValidatableObject validatableRequest)
            {
                var results = validatableRequest.Validate(new ValidationContext(validatableRequest));
                if (results.Any())
                {
                    return BadRequest(results.Select(r => r.ErrorMessage));
                }
            }

            var result = _mediator.Send(request);

            return Ok(result);
        }


    }
}
