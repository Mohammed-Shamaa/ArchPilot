using ArchPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArchPilot.Persistence.Configurations;

public class GeneratedDocumentConfiguration : IEntityTypeConfiguration<GeneratedDocument>
{
    public void Configure(EntityTypeBuilder<GeneratedDocument> builder)
    {
        builder.HasKey(d => d.Id);
        builder.HasIndex(d => new { d.ProjectId, d.DocumentType });
        builder.Property(d => d.Title).HasMaxLength(200).IsRequired();
        builder.Property(d => d.Content).IsRequired();
        builder.Property(d => d.Format).HasMaxLength(50).HasDefaultValue("markdown");
        builder.HasOne(d => d.Project).WithMany(p => p.GeneratedDocuments).HasForeignKey(d => d.ProjectId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(d => d.Conversation).WithMany().HasForeignKey(d => d.ConversationId).OnDelete(DeleteBehavior.SetNull);
    }
}
