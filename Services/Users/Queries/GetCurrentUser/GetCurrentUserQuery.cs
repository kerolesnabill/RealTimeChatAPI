using MediatR;
using RealTimeChatAPI.DTOs;

namespace RealTimeChatAPI.Services.Users.Queries.GetCurrentUser;

public class GetCurrentUserQuery : IRequest<UserDto>
{
}
