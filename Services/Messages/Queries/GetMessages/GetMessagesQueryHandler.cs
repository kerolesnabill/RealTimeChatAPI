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
        IMessagesRepository messagesRepository, 
        IUsersRepository usersRepository,
        IUserContext userContext,
        IMapper mapper) : IRequestHandler<GetMessagesQuery, IEnumerable<MessageDto>>
{
    public async Task<IEnumerable<MessageDto>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.CurrentUser();
        logger.LogInformation("Getting chat messages for user {UserId} by user {CurrentUserId}", request.UserId, currentUser.Id);

        var user = await usersRepository.GetByIdAsync(request.UserId)
                ?? throw new NotFoundException(nameof(User), request.UserId.ToString());

        var messages = await messagesRepository.GetMessagesAsync(currentUser.Id ,request.UserId);

        return mapper.Map<IEnumerable<MessageDto>>(messages);
    }
}
