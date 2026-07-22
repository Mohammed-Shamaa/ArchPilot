namespace ArchPilot.Domain.Entities;

public class Team
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid OwnerId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User Owner { get; set; } = null!;
    public ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
    public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
}
