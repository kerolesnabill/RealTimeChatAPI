using Application.Services;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.LoginUser;

public class LoginUserCommandHandler(
        ILogger<LoginUserCommandHandler> logger,
        IUsersRepository usersRepository,
        TokenService tokenService) : IRequestHandler<LoginUserCommand, string>
{
    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Login user: {Username}", request.Username);

        var user = await usersRepository.GetByUsernameAsync(request.Username)
            ?? throw new InvalidLoginException();

        bool correctPass = BCrypt.Net.BCrypt
            .EnhancedVerify(request.Password, user.HashedPassword);
        if (!correctPass) throw new InvalidLoginException();

        return tokenService.GenerateToken(user);
    }
}
