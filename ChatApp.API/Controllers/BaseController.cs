using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator ;

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
