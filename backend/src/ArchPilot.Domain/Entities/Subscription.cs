using ArchPilot.Domain.Enums;

namespace ArchPilot.Domain.Entities;

public class Subscription
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public SubscriptionPlan PlanName { get; set; } = SubscriptionPlan.Free;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
    public string PaymentStatus { get; set; } = "pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
}
