using MediatR;
using RealTimeChatAPI.Data.Repositories;
using RealTimeChatAPI.Exceptions;
using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Services.Users.Commands.UpdateUserPassword;

public class UpdateUserPasswordCommandHandler(
        ILogger<UpdateUserPasswordCommand> logger,
        IUsersRepository usersRepository,
        IUserContext userContext) : IRequestHandler<UpdateUserPasswordCommand>
{
    public async Task Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.CurrentUser();
        logger.LogInformation("User: {UserId} is updating his password", currentUser.Id);

        var user = await usersRepository.GetByIdAsync(currentUser.Id)
            ?? throw new NotFoundException(nameof(User), currentUser.Id.ToString());

        bool isCorrectPass = BCrypt.Net.BCrypt.EnhancedVerify(request.CurrentPassword, user.HashedPassword);
        if (!isCorrectPass) throw new Exception("Current password is incorrect");

        user.HashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(request.NewPassword);

        await usersRepository.UpdateAsync(user);
    }
}