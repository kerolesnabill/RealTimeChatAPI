using Application.Users.Commands.RegisterUser;
using Application.Users.Commands.UpdateUser;
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

        CreateMap<UpdateUserCommand, User>()
            .ForMember(u => u.Name, options =>
                options.Condition(c => c.Name != null))
            .ForMember(u => u.About, options =>
                options.Condition(c => c.About != null))
            .ForMember(u => u.Username, options =>
                options.Condition(c => c.Username?.ToLower() != null));

        CreateMap<User, UserDto>();
    }
}
