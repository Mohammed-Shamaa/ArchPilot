using ArchPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArchPilot.Persistence.Configurations;

public class DocumentVersionConfiguration : IEntityTypeConfiguration<DocumentVersion>
{
    public void Configure(EntityTypeBuilder<DocumentVersion> builder)
    {
        builder.HasKey(v => v.Id);
        builder.HasIndex(v => new { v.DocumentId, v.VersionNumber });
        builder.Property(v => v.Content).IsRequired();
        builder.HasOne(v => v.Document).WithMany(d => d.DocumentVersions).HasForeignKey(v => v.DocumentId).OnDelete(DeleteBehavior.Cascade);
    }
}
