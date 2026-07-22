using ArchPilot.Domain.Enums;

namespace ArchPilot.Application.Common.Models;

public class CurrentUser
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string LanguagePreference { get; set; } = "en";
}
