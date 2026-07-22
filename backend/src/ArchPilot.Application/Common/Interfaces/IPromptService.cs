using ArchPilot.Domain.Enums;

namespace ArchPilot.Application.Common.Interfaces;

public interface IPromptService
{
    Task<string> GetPromptAsync(AIAgentType agentType, CancellationToken cancellationToken = default);
    Task UpdatePromptAsync(AIAgentType agentType, string promptContent, CancellationToken cancellationToken = default);
}
