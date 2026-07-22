using ArchPilot.Domain.Entities;

namespace ArchPilot.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    Guid? ValidateRefreshToken(string tokenHash);
}
