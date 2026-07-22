using ArchPilot.Domain.Enums;

namespace ArchPilot.Application.Common.Interfaces;

public interface IAIContextService
{
    Task<string> GetProjectContextAsync(Guid projectId, CancellationToken cancellationToken = default);
    Task AddContextAsync(Guid projectId, ContextType contextType, string content, CancellationToken cancellationToken = default);
    Task<List<string>> GetRecentContextAsync(Guid projectId, int count = 5, CancellationToken cancellationToken = default);
}
