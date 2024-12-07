using MediatR;
using RealTimeChatAPI.Data.Repositories;
using RealTimeChatAPI.Exceptions;
using RealTimeChatAPI.Helpers;

namespace RealTimeChatAPI.Services.Users.Commands.LoginUser;

public class LoginUserCommandHandler(
        ILogger<LoginUserCommandHandler> logger,
        IUsersRepository usersRepository,
        JwtHelper jwtHelper) : IRequestHandler<LoginUserCommand, string>
{
    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Login user: {Username}", request.Username);

        var user = await usersRepository.GetUserByUsername(request.Username)
            ?? throw new InvalidLoginException();

        bool correctPass = BCrypt.Net.BCrypt
            .EnhancedVerify(request.Password, user.HashedPassword);
        if (!correctPass) throw new InvalidLoginException();

        return jwtHelper.GenerateToken(user);
    }
}
