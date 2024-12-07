using AutoMapper;
using MediatR;
using RealTimeChatAPI.Data.Repositories;
using RealTimeChatAPI.Exceptions;
using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Services.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler(
        ILogger<RegisterUserCommandHandler> logger,
        IUsersRepository usersRepository,
        IMapper mapper) : IRequestHandler<RegisterUserCommand>
{
    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Register a new User {@User}", request);

        var existingUser = await usersRepository.GetUserByUsername(request.Username);
        if (existingUser != null) throw new UsernameAlreadyUsedException(request.Username);

        User user = mapper.Map<User>(request);
        user.CreatedAt = DateTime.UtcNow;
        user.HashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);

        await usersRepository.Add(user);
    }
}
