using MediatR;

namespace RealTimeChatAPI.Services.Users.Commands.LoginUser;

public class LoginUserCommand : IRequest<string>
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}