using ArchPilot.Application.Common.Interfaces;
using ArchPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArchPilot.Persistence.Context;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<AIContext> AIContexts => Set<AIContext>();
    public DbSet<GeneratedDocument> GeneratedDocuments => Set<GeneratedDocument>();
    public DbSet<DocumentVersion> DocumentVersions => Set<DocumentVersion>();
    public DbSet<Prompt> Prompts => Set<Prompt>();
    public DbSet<AIUsage> AIUsages => Set<AIUsage>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<Invitation> Invitations => Set<Invitation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
