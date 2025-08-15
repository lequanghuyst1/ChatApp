using ChatApp.Application.DTOs;
using ChatApp.Application.DTOs.Profile;
using ChatApp.Application.Model;
using ChatApp.Application.UseCases.Friend.Commands;
using ChatApp.Application.UseCases.Friend.Queries;
using ChatApp.Application.UseCases.Profile.Queries;
using ChatApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ChatApp.API.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        public ProfileController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("get-profile")]
        public async Task<ActionResult<APIResponse<UserProfileDTO>>> GetProfile()
        {
            var result = await _mediator.Send(new GetProfileByIdQuery());
            return Ok(result);
        }

        [HttpGet("get-users")]
        public async Task<ActionResult<APIResponse<IEnumerable<UserProfileDTO>>>> GetUsers(string? keyword = "")
        {
            var result = await _mediator.Send(new GetUsersQuery(keyword));
            return Ok(result);
        }
    }
}
