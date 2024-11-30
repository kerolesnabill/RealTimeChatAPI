using MediatR;

namespace Application.Users.Commands.UpdateUserPassword;

public class UpdateUserPasswordCommand : IRequest
{
    public string CurrentPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}
