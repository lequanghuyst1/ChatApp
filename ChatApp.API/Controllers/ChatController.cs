using ChatApp.Application.DTOs;
using ChatApp.Application.Model;
using ChatApp.Application.UseCases.Chat.Commands;
using ChatApp.Application.UseCases.Chat.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : BaseController
    {
        public ChatController(IMediator mediator) : base(mediator)
        {

        }

        [HttpGet("get-list")]
        public async Task<ActionResult<APIResponse<IEnumerable<ChatDTO>>>> GetChatList()
        {
            var result = await _mediator.Send(new GetChatsByUserQuery());
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<ActionResult<APIResponse>> CreateChat([FromBody] CreateChatCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<ActionResult<APIResponse>> UpdateChat([FromBody] UpdateChatCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<APIResponse>> DeleteChat(long id)
        {
            var command = new DeleteChatCommand(){ChatID = id};
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("add-participant")]
        public async Task<ActionResult<APIResponse>> AddParticipant([FromBody] AddParticipantCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("leave")]
        public async Task<ActionResult<APIResponse>> LeaveChat(long id)
        {
            var command = new LeaveChatCommand(){ChatID = id};
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
