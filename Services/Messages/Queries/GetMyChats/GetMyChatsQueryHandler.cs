using MediatR;
using RealTimeChatAPI.Data.Repositories;
using RealTimeChatAPI.DTOs;
using RealTimeChatAPI.Services.Users;

namespace RealTimeChatAPI.Services.Messages.Queries.GetMyChats;

public class GetMyChatsQueryHandler(
        ILogger<GetMyChatsQueryHandler> logger,
        IMessagesRepository messagesRepository,
        IUserContext userContext
        ) : IRequestHandler<GetMyChatsQuery, IEnumerable<ChatRoomDto>>
{
    public async Task<IEnumerable<ChatRoomDto>> Handle(GetMyChatsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.CurrentUser();
        logger.LogInformation("Getting chat rooms for user {UserId}", currentUser.Id);

        var chatRooms = await messagesRepository.GetChatRoomsAsync(currentUser.Id);

        return chatRooms;
    }
}
