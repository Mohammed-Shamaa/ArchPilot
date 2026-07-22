using ArchPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArchPilot.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.UserId);
        builder.HasIndex(p => p.LastAccessedAt);
        builder.Property(p => p.ProjectName).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(2000);
        builder.HasOne(p => p.User).WithMany(u => u.Projects).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
