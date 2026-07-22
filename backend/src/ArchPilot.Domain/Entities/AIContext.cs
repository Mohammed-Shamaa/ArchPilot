using ArchPilot.Domain.Enums;

namespace ArchPilot.Domain.Entities;

public class AIContext
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public ContextType ContextType { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Project Project { get; set; } = null!;
}
