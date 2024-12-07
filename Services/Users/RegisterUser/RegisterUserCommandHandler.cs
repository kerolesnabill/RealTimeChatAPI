using AutoMapper;
using MediatR;
using RealTimeChatAPI.Data.Repositories;
using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Services.Users.RegisterUser;

public class RegisterUserCommandHandler(
        ILogger<RegisterUserCommandHandler> logger,
        IUsersRepository usersRepository,
        IMapper mapper) : IRequestHandler<RegisterUserCommand>
{
    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Register a new User {@User}", request);

        // TODO: check if the username already exists

        User user = mapper.Map<User>(request);
        user.CreatedAt = DateTime.UtcNow;
        user.HashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);

        await usersRepository.Add(user);
    }
}
