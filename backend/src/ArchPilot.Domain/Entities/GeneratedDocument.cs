using ArchPilot.Domain.Enums;

namespace ArchPilot.Domain.Entities;

public class GeneratedDocument
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? ConversationId { get; set; }
    public DocumentType DocumentType { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Format { get; set; } = "markdown";
    public int Version { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Project Project { get; set; } = null!;
    public Conversation? Conversation { get; set; }
    public ICollection<DocumentVersion> DocumentVersions { get; set; } = new List<DocumentVersion>();
}
