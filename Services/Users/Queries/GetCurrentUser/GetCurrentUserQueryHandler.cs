using AutoMapper;
using MediatR;
using RealTimeChatAPI.Data.Repositories;
using RealTimeChatAPI.DTOs;
using RealTimeChatAPI.Exceptions;
using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Services.Users.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler(
        ILogger<GetCurrentUserQueryHandler> logger,
        IUsersRepository usersRepository,
        IUserContext userContext,
        IMapper mapper) : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.CurrentUser();
        logger.LogInformation("Getting current user, UserId: {UserId}", currentUser.Id);

        var user = await usersRepository.GetByIdAsync(currentUser.Id)
            ?? throw new NotFoundException(nameof(User), currentUser.Id.ToString());

        return mapper.Map<UserDto>(user);
    }
}
