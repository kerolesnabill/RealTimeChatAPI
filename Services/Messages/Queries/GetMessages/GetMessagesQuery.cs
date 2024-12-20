using MediatR;
using RealTimeChatAPI.DTOs;

namespace RealTimeChatAPI.Services.Messages.Queries.GetMessages;

public class GetMessagesQuery : IRequest<IEnumerable<MessageDto>>
{
    public Guid UserId { get; set; } = default!;
}