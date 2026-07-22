using ArchPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArchPilot.Persistence.Configurations;

public class AIUsageConfiguration : IEntityTypeConfiguration<AIUsage>
{
    public void Configure(EntityTypeBuilder<AIUsage> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasIndex(u => new { u.UserId, u.CreatedAt });
        builder.Property(u => u.ModelName).HasMaxLength(100).IsRequired();
        builder.Property(u => u.RequestCost).HasPrecision(18, 6);
        builder.HasOne(u => u.User).WithMany(u => u.AIUsages).HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(u => u.Project).WithMany(p => p.AIUsages).HasForeignKey(u => u.ProjectId).OnDelete(DeleteBehavior.Cascade);
    }
}
