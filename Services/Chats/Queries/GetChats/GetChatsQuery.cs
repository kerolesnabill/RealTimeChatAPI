using MediatR;
using RealTimeChatAPI.DTOs;

namespace RealTimeChatAPI.Services.Chats.Queries.GetChats;

public class GetChatsQuery : IRequest<IEnumerable<ChatDto?>>
{
}
