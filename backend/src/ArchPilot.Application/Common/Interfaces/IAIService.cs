using ArchPilot.Domain.Enums;

namespace ArchPilot.Application.Common.Interfaces;

public interface IAIService
{
    Task<string> ChatAsync(string message, string context, AIAgentType agentType, string language = "en", CancellationToken cancellationToken = default);
    Task<string> GenerateDocumentAsync(string projectDescription, DocumentType documentType, string context, string language = "en", CancellationToken cancellationToken = default);
}
