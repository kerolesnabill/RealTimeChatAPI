using AutoMapper;
using RealTimeChatAPI.DTOs;
using RealTimeChatAPI.Models;
using RealTimeChatAPI.Services.Users.Commands.RegisterUser;

namespace RealTimeChatAPI.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();

        CreateMap<RegisterUserCommand, User>()
            .ForMember(u => u.Username,
                options => options.MapFrom(c => c.Username.ToLower())); ;
    }
}
