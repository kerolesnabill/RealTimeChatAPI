using MediatR;
using RealTimeChatAPI.DTOs;

namespace RealTimeChatAPI.Services.Messages.Queries.GetMyChats;

public class GetMyChatsQuery : IRequest<IEnumerable<ChatRoomDto>>
{
}
