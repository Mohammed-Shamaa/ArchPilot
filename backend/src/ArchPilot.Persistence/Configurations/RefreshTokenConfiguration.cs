using ArchPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArchPilot.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(r => r.Id);
        builder.HasIndex(r => r.TokenHash);
        builder.HasIndex(r => new { r.UserId, r.IsRevoked });
        builder.Property(r => r.TokenHash).HasMaxLength(500).IsRequired();
        builder.HasOne(r => r.User).WithMany(u => u.RefreshTokens).HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
