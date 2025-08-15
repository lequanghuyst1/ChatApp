using ChatApp.Application.DTOs;
using ChatApp.Application.Model;
using ChatApp.Application.UseCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Authorize]
    public class ReactionController : BaseController
    {
        public ReactionController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("get-list-by-message")]
        public async Task<ActionResult<APIResponse<IEnumerable<ReactionDTO>>>> GetReactionsByMessage([FromQuery] long messageID)
        {
            var query = new GetReactionsByMessageQuery { MessageID = messageID };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<ActionResult<APIResponse<long>>> AddReaction([FromBody] AddReactionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<ActionResult<APIResponse>> EditReaction([FromBody] EditMessageCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("remove")]
        public async Task<ActionResult<APIResponse>> RemoveReaction([FromBody] RemoveReactionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
