using MediatR;

namespace RealTimeChatAPI.Services.Users.Commands.UpdateUserPassword;

public class UpdateUserPasswordCommand : IRequest
{
    public string CurrentPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}
