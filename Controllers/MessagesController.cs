using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeChatAPI.Services.Messages.Queries.GetMessages;

namespace RealTimeChatAPI.Controllers;

[ApiController]
[Route("api/messages")]
[Authorize]
public class MessagesController(IMediator mediator) : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetMessage([FromRoute] GetMessagesQuery query)
    {
        var messages = await mediator.Send(query);
        return Ok(messages);
    }
}
