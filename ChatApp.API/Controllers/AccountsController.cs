using ChatApp.Application.Model;
using ChatApp.Application.UseCases.Account.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController
    {

        public AccountsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("login")]
        public async Task<APIResponse<LoginResponse>> Login([FromBody] LoginUserCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("register")]
        public async Task<APIResponse<RegisterResponse>> Register([FromBody] RegisterUserCommand command)
        {
            return  await _mediator.Send(command);
        }
      
    }
}
