using System.Security.Claims;
using ArchPilot.Application.Common.Interfaces;
using ArchPilot.Application.Common.Models;
using ArchPilot.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace ArchPilot.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrentUser? GetCurrentUser()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null || !(user.Identity?.IsAuthenticated ?? false))
            return null;

        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(idClaim) || !Guid.TryParse(idClaim, out var id))
            return null;

        return new CurrentUser
        {
            Id = id,
            Username = user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty,
            Email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
            Role = Enum.TryParse<UserRole>(user.FindFirst(ClaimTypes.Role)?.Value, out var role) ? role : UserRole.User,
            LanguagePreference = user.FindFirst("LanguagePreference")?.Value ?? "en"
        };
    }
}
