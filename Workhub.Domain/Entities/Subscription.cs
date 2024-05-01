namespace Workhub.Domain.Entities;

public class Subscription
{
    public Guid SubscriptionId { get; set; } = Guid.NewGuid();
    public bool IsEnabled { get; set; }
    public double AmountInDollars { get; set; }
    public double AmountInNaira { get; set; }
}
