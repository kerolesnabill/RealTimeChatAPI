using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RealTimeChatAPI.Services.Users;

public interface IUserContext
{
    CurrentUser CurrentUser();
}

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser CurrentUser()
    {
        var user = httpContextAccessor?.HttpContext?.User
            ?? throw new InvalidOperationException("User context is not present");

        if (user.Identity == null || !user.Identity.IsAuthenticated)
            throw new UnauthorizedAccessException("User is not authenticated");

        var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var username = user.FindFirst(c => c.Type == ClaimTypes.Name)!.Value;
        var name = user.FindFirst(c => c.Type == JwtRegisteredClaimNames.Name)!.Value;

        return new CurrentUser(Guid.Parse(userId), username, name);
    }
}
