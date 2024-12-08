using AutoMapper;
using MediatR;
using RealTimeChatAPI.Data.Repositories;
using RealTimeChatAPI.DTOs;
using RealTimeChatAPI.Exceptions;
using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Services.Users.Queries.GetUserByUsername;

public class GetUserByUsernameQueryHandler(
        ILogger<GetUserByUsernameQueryHandler> logger,
        IUsersRepository usersRepository,
        IMapper mapper) : IRequestHandler<GetUserByUsernameQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting user with username: {Username}", request.Username);

        var user = await usersRepository.GetByUsernameAsync(request.Username)
            ?? throw new NotFoundException(nameof(User), request.Username);

        return mapper.Map<UserDto>(user);
    }
}