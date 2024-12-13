using AutoMapper;
using MediatR;
using RealTimeChatAPI.Data.Repositories;
using RealTimeChatAPI.DTOs;
using RealTimeChatAPI.Services.Users;

namespace RealTimeChatAPI.Services.Chats.Queries.GetChats;

public class GetChatsQueryHandler(
        ILogger<GetChatsQueryHandler> logger,
        IChatsRepository chatsRepository,
        IUserContext userContext,
        IMapper mapper) : IRequestHandler<GetChatsQuery, IEnumerable<ChatDto?>>
{
    public async Task<IEnumerable<ChatDto?>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.CurrentUser();
        logger.LogInformation("Getting chats for user: {UserId}", currentUser.Id);

        return await chatsRepository.GetChatsByUserIdAsync(currentUser.Id);
    }
}
