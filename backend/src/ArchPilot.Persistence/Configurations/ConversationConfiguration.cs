using ArchPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArchPilot.Persistence.Configurations;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.ProjectId);
        builder.Property(c => c.Title).HasMaxLength(200).IsRequired();
        builder.HasOne(c => c.Project).WithMany(p => p.Conversations).HasForeignKey(c => c.ProjectId).OnDelete(DeleteBehavior.Cascade);
    }
}
