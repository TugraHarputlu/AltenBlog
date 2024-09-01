using AltenBlog.Common.Models.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AltenBlog.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPatch]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var res = await mediator.Send(command);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateUserComment comment)
        {
            var guid = await mediator.Send(comment);

            return Ok(guid);
        }
        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult> Update([FromBody] UpdateUserCommand comment)
        {
            var guid = await mediator.Send(comment);

            return Ok(guid);
        }
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetAll()
        {

            return Ok("Alles OK");
        }
    }
}
