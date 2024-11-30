using Domain.Constants;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.UpdateUserImage;

public class UpdateUserImageCommandHandler(
        ILogger<UpdateUserImageCommandHandler> logger,
        IBlobStorageService blobStorageService,
        IUsersRepository usersRepository,
        IUserContext userContext) : IRequestHandler<UpdateUserImageCommand>
{
    public async Task Handle(UpdateUserImageCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.CurrentUser();
        logger.LogInformation("User: {UserId} is updating his image", currentUser.Id);

        var user = await usersRepository.GetByIdAsync(currentUser.Id)
            ?? throw new NotFoundException(nameof(User), currentUser.Id.ToString());

        if(request.Image == null || request.Image.Length == 0)
            throw new Exception("No Image file uploaded.");

        List<string> allowedMimeTypes = 
            ["image/jpeg", "image/png", "image/gif", "image/bmp", "image/webp"];

        if(!allowedMimeTypes.Contains(request.Image.ContentType))
            throw new Exception("Invalid file type. Only image files are allowed.");

        long maxSize = 5 * 1024 * 1024; // 5 MB
        if (request.Image.Length > maxSize)
            throw new Exception("Image size exceeds the 5MB limit.");

        using var stream = request.Image.OpenReadStream();
        string filename = $"user-{user.Id}-{DateTime.Now.GetHashCode()}.jpeg";

        var imageUrl = await blobStorageService.UploadToBlobAsync
            (stream, filename, ContainerName.Users);

        user.Image = imageUrl;
        await usersRepository.UpdateAsync(user);
    }
}
