using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using RealTimeChatAPI.Models;
using System.Security.Claims;
using System.Text;

namespace RealTimeChatAPI.Helpers;

public class JwtHelper(IConfiguration configuration)
{
    public string GenerateToken(User user)
    {
        List<Claim> claims =
            [
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, user.Username),
                new(JwtRegisteredClaimNames.Name, user.Name)
            ];

        string secret = configuration["jwt:Secret"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(configuration.GetValue<int>("jwt:ExpirationInMinutes")),
            SigningCredentials = credentials,
            Issuer = configuration["jwt:Issuer"],
            Audience = configuration["jwt:Audience"]
        };

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);

        return token;
    }
}

