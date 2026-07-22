using ArchPilot.Domain.Enums;

namespace ArchPilot.Domain.Entities;

public class Invitation
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public string Email { get; set; } = string.Empty;
    public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Team Team { get; set; } = null!;
}
