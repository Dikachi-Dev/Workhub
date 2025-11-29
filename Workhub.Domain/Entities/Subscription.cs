namespace Workhub.Domain.Entities;

public class Subscription
{
    public Guid SubscriptionId { get; set; }
    public bool IsEnabled { get; set; }
    public double AmountInDollars { get; set; }
    public double AmountInNaira { get; set; }
    public string PayPalKey { get; set; }
    public string PayPalSecret { get; set; }
    public string NokoKashId { get; set; }
}
