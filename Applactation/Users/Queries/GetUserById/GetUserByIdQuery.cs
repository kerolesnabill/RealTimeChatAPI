using Application.Users.Dtos;
using MediatR;

namespace Application.Users.Queries.GetUserById;

public class GetUserByIdQuery(Guid id) : IRequest<UserDto>
{
    public Guid Id { get; set; } = id;
}
