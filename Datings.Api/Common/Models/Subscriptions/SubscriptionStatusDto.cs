namespace Datings.Api.Common.Models.Subscriptions;

public class SubscriptionStatusDto : SubscriptionDto
{
    public bool IsActive => ExpiredAt.HasValue && ExpiredAt >= DateTime.UtcNow;
    public DateTime? ExpiredAt { get; set; }
}