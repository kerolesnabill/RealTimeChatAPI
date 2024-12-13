using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using RealTimeChatAPI.Services.Chats.Queries.GetChats;

namespace RealTimeChatAPI.Controllers;

[ApiController]
[Route("api/chats")]
[Authorize]
public class ChatsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetChats()
    {
        var chats = await mediator.Send(new GetChatsQuery());
        return Ok(chats);
    }
}
