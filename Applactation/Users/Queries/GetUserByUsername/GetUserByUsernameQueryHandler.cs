using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.GetUserByUsername;

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
