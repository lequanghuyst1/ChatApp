using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

using System.Threading.Tasks;
using ChatApp.Application.UseCases;
using ChatApp.Application.Model;
using ChatApp.Application.DTOs;

namespace ChatApp.API.Controllers
{
    [Authorize]
    public class MessageController : BaseController
    {
        public MessageController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("send")]
        public async Task<ActionResult<APIResponse<long>>> SendMessage([FromBody] SendMessageCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<ActionResult<APIResponse>> EditMessage(long id, [FromBody] EditMessageCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<APIResponse>> DeleteMessage(long id)
        {
            var command = new DeleteMessageCommand { ID = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("get-list-by-chat")]
        public async Task<ActionResult<APIResponse<IEnumerable<MessageDTO>>>> GetMessagesByChat(long chatId, int page = 1, int pageSize = 20)
        {
            var query = new GetMessagesByChatQuery { ChatID = chatId, Page = page, PageSize = pageSize };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}