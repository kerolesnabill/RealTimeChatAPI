using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.RegisterUser;

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

        // TODO: hash password
        user.HashedPassword = request.Password;

        await usersRepository.Add(user);
    }
}
