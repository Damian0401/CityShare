using CityShare.Backend.Application.Core.Abstractions.Auth;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CityShare.Backend.Infrastructure.Auth;

public class JwtProvider : IJwtProvider
{
    private readonly AuthSettings _jwtSettings;

    public JwtProvider(IOptions<AuthSettings> options)
    {
        _jwtSettings = options.Value;
    }

    public string GenerateToken(ApplicationUser user, IEnumerable<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey));

        var claims = new List<Claim>
        {
            new (ClaimTypes.Email, user.Email ?? string.Empty),
            new (ClaimTypes.NameIdentifier, user.Id),
            new (ClaimTypes.Name, user.UserName ?? string.Empty),
        };

        foreach (var role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            SigningCredentials = creds,
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string? GetEmailFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(token))
        {
            return null;
        };

        var jwtSecurityToken = handler.ReadJwtToken(token);

        return jwtSecurityToken.Claims
            .FirstOrDefault(x => x.Type.Equals(nameof(ClaimTypes.Email), StringComparison.OrdinalIgnoreCase))?.Value;
    }
}
