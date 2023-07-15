using CityShare.Backend.Application.Core.Abstractions.Authentication;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CityShare.Backend.Infrastructure.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly JwtSettings _jwtSettings;

    public JwtProvider(IOptions<JwtSettings> options)
    {
        _jwtSettings = options.Value;
    }

    public string GenerateToken(ApplicationUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey));

        var claims = new List<Claim>
        {
            new (ClaimTypes.Email, user.Email ?? string.Empty)
        };
        
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
}
