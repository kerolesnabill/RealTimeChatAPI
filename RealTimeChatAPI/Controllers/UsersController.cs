using Application.Users.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealTimeChatAPI.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterUser(RegisterUserCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }
}
