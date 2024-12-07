using AutoMapper;
using RealTimeChatAPI.Models;
using RealTimeChatAPI.Services.Users.RegisterUser;

namespace RealTimeChatAPI.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterUserCommand, User>()
            .ForMember(u => u.Username,
                options => options.MapFrom(c => c.Username.ToLower())); ;
    }
}
