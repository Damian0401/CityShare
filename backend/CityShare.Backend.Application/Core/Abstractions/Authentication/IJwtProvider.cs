using CityShare.Backend.Domain.Entities;

namespace CityShare.Backend.Application.Core.Abstractions.Authentication;

public interface IJwtProvider
{
    string GenerateToken(ApplicationUser user);
}
