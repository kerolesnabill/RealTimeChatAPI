using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatAPI.Services.Users.Commands.LoginUser;
using RealTimeChatAPI.Services.Users.Commands.RegisterUser;

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

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginUser(LoginUserCommand command)
    {
        var token = await mediator.Send(command);
        return Ok(new { token });
    }
}