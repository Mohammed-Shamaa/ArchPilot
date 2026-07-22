using ArchPilot.Domain.Enums;

namespace ArchPilot.Domain.Entities;

public class TeamMember
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public Guid UserId { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    public Team Team { get; set; } = null!;
    public User User { get; set; } = null!;
}
