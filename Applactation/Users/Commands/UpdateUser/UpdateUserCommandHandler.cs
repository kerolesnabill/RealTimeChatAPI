using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(
        ILogger<UpdateUserCommandHandler> logger,
        IUsersRepository usersRepository,
        IUserContext userContext,
        IMapper mapper) : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.CurrentUser();
        logger.LogInformation("User {UserId} is updating his profile", currentUser.Id);

        var user = await usersRepository.GetByIdAsync(currentUser.Id)
            ?? throw new NotFoundException(nameof(User), currentUser.Id.ToString());

        mapper.Map(request, user);

        await usersRepository.UpdateAsync(user);
    }
}
