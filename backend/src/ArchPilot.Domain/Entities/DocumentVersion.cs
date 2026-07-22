namespace ArchPilot.Domain.Entities;

public class DocumentVersion
{
    public Guid Id { get; set; }
    public Guid DocumentId { get; set; }
    public int VersionNumber { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? ChangeSummary { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public GeneratedDocument Document { get; set; } = null!;
}
