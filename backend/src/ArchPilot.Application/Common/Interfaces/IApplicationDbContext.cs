using ArchPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArchPilot.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Project> Projects { get; }
    DbSet<Conversation> Conversations { get; }
    DbSet<Message> Messages { get; }
    DbSet<AIContext> AIContexts { get; }
    DbSet<GeneratedDocument> GeneratedDocuments { get; }
    DbSet<DocumentVersion> DocumentVersions { get; }
    DbSet<Prompt> Prompts { get; }
    DbSet<AIUsage> AIUsages { get; }
    DbSet<Subscription> Subscriptions { get; }
    DbSet<Team> Teams { get; }
    DbSet<TeamMember> TeamMembers { get; }
    DbSet<Invitation> Invitations { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
