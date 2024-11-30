using Application.Users.Dtos;
using MediatR;

namespace Application.Users.Queries.GetCurrentUser;

public class GetCurrentUserQuery : IRequest<UserDto>
{
}
