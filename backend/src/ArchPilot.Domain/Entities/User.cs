using ArchPilot.Domain.Enums;

namespace ArchPilot.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? ProfileImage { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public string LanguagePreference { get; set; } = "en";
    public string ThemePreference { get; set; } = "light";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<AIUsage> AIUsages { get; set; } = new List<AIUsage>();
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
