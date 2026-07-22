using ArchPilot.Domain.Enums;

namespace ArchPilot.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
    public ICollection<AIContext> AIContexts { get; set; } = new List<AIContext>();
    public ICollection<GeneratedDocument> GeneratedDocuments { get; set; } = new List<GeneratedDocument>();
    public ICollection<AIUsage> AIUsages { get; set; } = new List<AIUsage>();
}
