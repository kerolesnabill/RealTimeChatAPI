using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatAPI.Services.Messages.Queries.GetMessages;

namespace RealTimeChatAPI.Controllers;

[ApiController]
[Route("api/chats/{chatId:guid}/messages")]
[Authorize]
public class MessagesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMessages([FromRoute] Guid chatId)
    {
        var messages = await mediator.Send(new GetMessagesQuery(chatId));
        return Ok(messages);
    }
}
