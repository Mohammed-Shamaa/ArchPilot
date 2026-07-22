using ArchPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArchPilot.Persistence.Configurations;

public class AIContextConfiguration : IEntityTypeConfiguration<AIContext>
{
    public void Configure(EntityTypeBuilder<AIContext> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => new { c.ProjectId, c.ContextType });
        builder.Property(c => c.Content).IsRequired();
        builder.HasOne(c => c.Project).WithMany(p => p.AIContexts).HasForeignKey(c => c.ProjectId).OnDelete(DeleteBehavior.Cascade);
    }
}
