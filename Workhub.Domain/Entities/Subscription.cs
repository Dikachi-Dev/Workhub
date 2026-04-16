namespace Workhub.Domain.Entities;

public class Subscription
{
    public Guid SubscriptionId { get; set; }
    public bool IsEnabled { get; set; }
    public double AmountInDollars { get; set; }
    public double AmountInNaira { get; set; }
    public string PayPalKey { get; set; } = string.Empty;
    public string PayPalSecret { get; set; } = string.Empty;
    public string NokoKashId { get; set; } = string.Empty;
}
