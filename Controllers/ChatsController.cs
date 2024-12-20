using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatAPI.Services.Messages.Queries.GetMyChats;

namespace RealTimeChatAPI.Controllers;

[Route("api/chats")]
[ApiController]
[Authorize]
public class ChatsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMyChats()
    {
        var chats = await mediator.Send(new GetMyChatsQuery());
        return Ok(chats);
    }
}
