using Microsoft.AspNetCore.Mvc;
using ChatApp.Application.DTOs;
using ChatApp.Application.Model;
using MediatR;
using ChatApp.Application.UseCases;

namespace ChatApp.API.Controllers
{
    public class ReactionController : BaseController
    {
        public ReactionController(Mediator mediator) : base(mediator)
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
