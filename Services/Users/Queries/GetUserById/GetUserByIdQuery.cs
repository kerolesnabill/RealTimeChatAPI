using MediatR;
using RealTimeChatAPI.DTOs;

namespace RealTimeChatAPI.Services.Users.Queries.GetUserById;

public class GetUserByIdQuery(Guid id) : IRequest<UserDto>
{
    public Guid Id { get; set; } = id;
}
