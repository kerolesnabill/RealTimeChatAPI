using Application.Users.Dtos;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Domain.Exceptions;
using Domain.Entities;

namespace Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(
        ILogger<GetUserByIdQueryHandler> logger,
        IUsersRepository usersRepository, 
        IMapper mapper) : IRequestHandler<GetUserByIdQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting user with Id: {UserId}", request.Id);
        
        var user = await usersRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(User), request.Id.ToString());

        return mapper.Map<UserDto>(user);
    }
}
