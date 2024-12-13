using MediatR;
using RealTimeChatAPI.DTOs;

namespace RealTimeChatAPI.Services.Messages.Queries.GetMessages;

public class GetMessagesQuery(Guid chatId) : IRequest<IEnumerable<MessageDto>>
{
    public Guid ChatId { get; set; } = chatId;
}
