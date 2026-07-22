using ArchPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArchPilot.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.Username);
        builder.Property(u => u.Username).HasMaxLength(100).IsRequired();
        builder.Property(u => u.Email).HasMaxLength(255).IsRequired();
        builder.Property(u => u.PasswordHash).HasMaxLength(500).IsRequired();
        builder.Property(u => u.ProfileImage).HasMaxLength(500);
        builder.Property(u => u.LanguagePreference).HasMaxLength(10).HasDefaultValue("en");
        builder.Property(u => u.ThemePreference).HasMaxLength(10).HasDefaultValue("light");
    }
}
