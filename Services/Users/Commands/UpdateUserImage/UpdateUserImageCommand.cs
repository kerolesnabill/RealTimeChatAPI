using MediatR;

namespace RealTimeChatAPI.Services.Users.Commands.UpdateUserImage;

public class UpdateUserImageCommand : IRequest
{
    public IFormFile Image { get; set; } = default!;
}