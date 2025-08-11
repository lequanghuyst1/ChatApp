using ChatApp.Application.DTOs;
using ChatApp.Application.Model;
using ChatApp.Application.UseCases.Friend.Commands;
using ChatApp.Application.UseCases.Friend.Queries;
using ChatApp.Application.UseCases.Profile.Queries;
using ChatApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace ChatApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : BaseController
    {

        public ProfileController(Mediator mediator) : base(mediator)
        {
        }

        [HttpGet("get-profile")]
        public async Task<ActionResult<APIResponse<UserProfile>>> GetProfile()
        {
            var result = await _mediator.Send(new GetProfileByIdQuery());
            return Ok(result);
        }

    }
}
