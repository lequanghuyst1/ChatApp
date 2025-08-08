using ChatApp.Application.DTOs;
using ChatApp.Application.Model;
using ChatApp.Application.UseCases.Friend.Commands;
using ChatApp.Application.UseCases.Friend.Queries;
using ChatApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace ChatApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FriendController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add")]
        public async Task<ActionResult<APIResponse>> AddFriend([FromBody] AddFriendCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("remove")]
        public async Task<ActionResult<APIResponse>> RemoveFriend([FromBody] RemoveFriendCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("accept")]
        public async Task<ActionResult<APIResponse>> AcceptFriend([FromBody] AcceptFriendCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("list")]
        public async Task<ActionResult<APIResponse<IEnumerable<UserFriend>>>> GetFriendList()
        {
            var query = new GetFriendListQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("list-request")]
        public async Task<ActionResult<APIResponse<IEnumerable<UserFriend>>>> GetListFriendRequests()
        {
            var query = new GetListFriendRequestQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
