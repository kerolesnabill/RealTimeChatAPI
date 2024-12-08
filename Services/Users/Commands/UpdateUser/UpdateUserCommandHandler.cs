using AutoMapper;
using MediatR;
using RealTimeChatAPI.Data.Repositories;
using RealTimeChatAPI.Exceptions;
using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Services.Users.Commands.UpdateUser;

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

        if(request.Username != null)
        {
            var u = await usersRepository.GetByUsernameAsync(request.Username);
            if (u != null) throw new UsernameAlreadyUsedException(request.Username);
        }

        mapper.Map(request, user);

        await usersRepository.UpdateAsync(user);
    }
}
