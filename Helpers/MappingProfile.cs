using AutoMapper;
using RealTimeChatAPI.DTOs;
using RealTimeChatAPI.Models;
using RealTimeChatAPI.Services.Users.Commands.RegisterUser;
using RealTimeChatAPI.Services.Users.Commands.UpdateUser;

namespace RealTimeChatAPI.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();

        CreateMap<RegisterUserCommand, User>()
            .ForMember(u => u.Username,
                options => options.MapFrom(c => c.Username.ToLower())); ;

        CreateMap<UpdateUserCommand, User>()
            .ForMember(u => u.Name, options =>
                options.Condition(c => c.Name != null))
            .ForMember(u => u.About, options =>
                options.Condition(c => c.About != null))
            .ForMember(u => u.Username, options =>
                options.Condition(c => c.Username?.ToLower() != null));
    }
}
