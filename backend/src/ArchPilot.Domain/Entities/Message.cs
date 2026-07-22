using ArchPilot.Domain.Enums;

namespace ArchPilot.Domain.Entities;

public class Message
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public SenderType SenderType { get; set; }
    public string Content { get; set; } = string.Empty;
    public int TokenUsage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Conversation Conversation { get; set; } = null!;
}
