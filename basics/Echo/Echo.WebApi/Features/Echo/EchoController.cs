using System;
using System.Net;
using System.Threading.Tasks;
using BuildingBlocks.Mediatr.Commands;
using Echo.Application.CommandSide.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Echo.WebApi.Features.Echo
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class EchoController : Controller
    {
        private readonly IMediator _mediator;

        public EchoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEcho(
            [FromBody] EchoCommand command,
            [FromHeader(Name = "x-requestid")] string requestId
        )
        {
            if (!Guid.TryParse(requestId, out var guid))
            {
                return BadRequest();
            }

            var identifiedCommand = new IdentifiedCommand<EchoCommand, string>(
                command,
                guid
            );

            return Ok(await _mediator.Send(identifiedCommand));
        }
    }
}
