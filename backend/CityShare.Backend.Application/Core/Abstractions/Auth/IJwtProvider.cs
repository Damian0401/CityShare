using CityShare.Backend.Domain.Entities;

namespace CityShare.Backend.Application.Core.Abstractions.Auth;

public interface IJwtProvider
{
    string GenerateToken(ApplicationUser user, IEnumerable<string> roles);
    string? GetEmailFromToken(string token);
}
