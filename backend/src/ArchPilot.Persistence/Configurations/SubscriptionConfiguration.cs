using ArchPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArchPilot.Persistence.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(s => s.Id);
        builder.HasIndex(s => new { s.UserId, s.Status });
        builder.Property(s => s.PaymentStatus).HasMaxLength(50);
        builder.HasOne(s => s.User).WithMany(u => u.Subscriptions).HasForeignKey(s => s.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
