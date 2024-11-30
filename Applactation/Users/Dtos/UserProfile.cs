using Application.Users.Commands.RegisterUser;
using AutoMapper;
using Domain.Entities;

namespace Application.Users.Dtos;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserCommand, User>()
            .ForMember(u => u.Username, 
                options => options.MapFrom(c => c.Username.ToLower()));;
        CreateMap<User, UserDto>();
    }
}
