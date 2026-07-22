using ArchPilot.Application.Common.Interfaces;
using ArchPilot.Domain.Entities;
using ArchPilot.Domain.Enums;

namespace ArchPilot.Infrastructure.AI;

public class AIContextService : IAIContextService
{
    private readonly IRepository<AIContext> _contextRepository;
    private readonly IRepository<GeneratedDocument> _documentRepository;
    private readonly IRepository<Conversation> _conversationRepository;

    public AIContextService(
        IRepository<AIContext> contextRepository,
        IRepository<GeneratedDocument> documentRepository,
        IRepository<Conversation> conversationRepository)
    {
        _contextRepository = contextRepository;
        _documentRepository = documentRepository;
        _conversationRepository = conversationRepository;
    }

    public async Task<string> GetProjectContextAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        var contexts = await _contextRepository.FindAsync(c => c.ProjectId == projectId, cancellationToken);
        var documents = await _documentRepository.FindAsync(d => d.ProjectId == projectId, cancellationToken);
        var conversations = await _conversationRepository.FindAsync(c => c.ProjectId == projectId, cancellationToken);

        var sb = new System.Text.StringBuilder();

        if (contexts.Any())
        {
            sb.AppendLine("=== Project Context ===");
            foreach (var ctx in contexts.OrderByDescending(c => c.UpdatedAt).Take(10))
            {
                sb.AppendLine($"[{ctx.ContextType}]: {ctx.Content}");
            }
        }

        if (documents.Any())
        {
            sb.AppendLine("\n=== Generated Documents ===");
            foreach (var doc in documents.OrderByDescending(d => d.CreatedAt))
            {
                sb.AppendLine($"[{doc.DocumentType}] {doc.Title} (v{doc.Version})");
            }
        }

        if (conversations.Any())
        {
            sb.AppendLine($"\n=== Project has {conversations.Count()} conversations ===");
        }

        return sb.ToString();
    }

    public async Task AddContextAsync(Guid projectId, ContextType contextType, string content, CancellationToken cancellationToken = default)
    {
        var context = new AIContext
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            ContextType = contextType,
            Content = content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _contextRepository.AddAsync(context, cancellationToken);
    }

    public async Task<List<string>> GetRecentContextAsync(Guid projectId, int count = 5, CancellationToken cancellationToken = default)
    {
        var contexts = await _contextRepository.FindAsync(c => c.ProjectId == projectId, cancellationToken);
        return contexts.OrderByDescending(c => c.UpdatedAt).Take(count).Select(c => c.Content).ToList();
    }
}
