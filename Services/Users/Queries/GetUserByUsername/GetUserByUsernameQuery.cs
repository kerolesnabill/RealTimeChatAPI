using MediatR;
using RealTimeChatAPI.DTOs;

namespace RealTimeChatAPI.Services.Users.Queries.GetUserByUsername;

public class GetUserByUsernameQuery(string username) : IRequest<UserDto>
{
    public string Username { get; set; } = username;
}
