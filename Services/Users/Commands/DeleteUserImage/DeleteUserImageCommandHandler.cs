using MediatR;
using RealTimeChatAPI.Data.Repositories;
using RealTimeChatAPI.Exceptions;
using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Services.Users.Commands.DeleteUserImage;

public class DeleteUserImageCommandHandler(
        ILogger<DeleteUserImageCommandHandler> logger,
        IUsersRepository usersRepository,
        IUserContext userContext)  : IRequestHandler<DeleteUserImageCommand>
{
    public async Task Handle(DeleteUserImageCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.CurrentUser();
        logger.LogInformation("User: {UserId} deleting his image", currentUser.Id);

        var user = await usersRepository.GetByIdAsync(currentUser.Id)
            ?? throw new NotFoundException(nameof(User), currentUser.Id.ToString());

        user.Image = null;
        await usersRepository.UpdateAsync(user);
    }
}
