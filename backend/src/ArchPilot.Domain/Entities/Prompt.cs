using ArchPilot.Domain.Enums;

namespace ArchPilot.Domain.Entities;

public class Prompt
{
    public Guid Id { get; set; }
    public AIAgentType AgentType { get; set; }
    public string PromptContent { get; set; } = string.Empty;
    public int Version { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
