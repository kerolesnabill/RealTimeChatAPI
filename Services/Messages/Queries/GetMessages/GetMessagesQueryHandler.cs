using AutoMapper;
using MediatR;
using RealTimeChatAPI.Data.Repositories;
using RealTimeChatAPI.DTOs;
using RealTimeChatAPI.Exceptions;
using RealTimeChatAPI.Models;
using RealTimeChatAPI.Services.Users;

namespace RealTimeChatAPI.Services.Messages.Queries.GetMessages;

public class GetMessagesQueryHandler(
        ILogger<GetMessagesQueryHandler> logger,
        IChatsRepository chatsRepository,
        IUserContext userContext,
        IMapper mapper) : IRequestHandler<GetMessagesQuery, IEnumerable<MessageDto>>
{
    public async Task<IEnumerable<MessageDto>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.CurrentUser();
        logger.LogInformation("Getting messages for chat: {ChatId} by user: {UserId}",
                request.ChatId, currentUser.Id);

        bool isChatMember = await chatsRepository.IsAChatMember(request.ChatId, currentUser.Id);
        if (!isChatMember) throw new Exception("You are not a member in this chat");

        var chat = await chatsRepository.GetByIdWithMessagesAsync(request.ChatId)
            ?? throw new NotFoundException(nameof(Chat), request.ChatId.ToString());

        return mapper.Map<IEnumerable<MessageDto>>(chat.Messages);
    }
}
