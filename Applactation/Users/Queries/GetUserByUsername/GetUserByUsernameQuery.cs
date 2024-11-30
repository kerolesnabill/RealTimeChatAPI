using Application.Users.Dtos;
using MediatR;

namespace Application.Users.Queries.GetUserByUsername;

public class GetUserByUsernameQuery(string username) : IRequest<UserDto>
{
    public string Username { get; set; } = username;
}
