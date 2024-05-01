namespace Workhub.Domain.Entities;

public class SubHistory : BaseEntity
{
    public string SubscriberId { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    //public double Amount { get; set; } = 0.00;
}
