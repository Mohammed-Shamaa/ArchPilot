using ArchPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArchPilot.Persistence.Configurations;

public class PromptConfiguration : IEntityTypeConfiguration<Prompt>
{
    public void Configure(EntityTypeBuilder<Prompt> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => new { p.AgentType, p.IsActive });
        builder.Property(p => p.PromptContent).IsRequired();
    }
}
