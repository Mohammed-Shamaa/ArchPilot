namespace ArchPilot.Domain.Entities;

public class AIUsage
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
    public string ModelName { get; set; } = string.Empty;
    public int TokensUsed { get; set; }
    public decimal RequestCost { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Project Project { get; set; } = null!;
}
