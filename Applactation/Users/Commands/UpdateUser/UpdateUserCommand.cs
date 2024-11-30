using MediatR;

namespace Application.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest
{
    public string? Name { get; set; }
    public string? Username { get; set; }
    public string? About { get; set; }
}
