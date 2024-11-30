using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Users.Commands.UpdateUserImage;

public class UpdateUserImageCommand(IFormFile image) : IRequest
{
    public IFormFile Image { get; set; } = image;
}
