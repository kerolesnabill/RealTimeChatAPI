using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatAPI.Services.Users.Commands.LoginUser;
using RealTimeChatAPI.Services.Users.Commands.RegisterUser;
using RealTimeChatAPI.Services.Users.Commands.UpdateUser;
using RealTimeChatAPI.Services.Users.Commands.UpdateUserImage;
using RealTimeChatAPI.Services.Users.Commands.UpdateUserPassword;
using RealTimeChatAPI.Services.Users.Queries.GetCurrentUser;
using RealTimeChatAPI.Services.Users.Queries.GetUserById;
using RealTimeChatAPI.Services.Users.Queries.GetUserByUsername;

namespace RealTimeChatAPI.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterUser(RegisterUserCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginUser(LoginUserCommand command)
    {
        var token = await mediator.Send(command);
        return Ok(new { token });
    }


    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var user = await mediator.Send(new GetUserByIdQuery(id));
        return Ok(user);
    }

    [HttpGet("{username}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserByUsername([FromRoute] string username)
    {
        var user = await mediator.Send(new GetUserByUsernameQuery(username));
        return Ok(user);
    }

    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await mediator.Send(new GetCurrentUserQuery());
        return Ok(user);
    }

    [HttpPatch("me")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateCurrentUser(UpdateUserCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("me/image")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateCurrentUserImage(UpdateUserImageCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPatch("me/password")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateCurrentUserPassword([FromBody] UpdateUserPasswordCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}