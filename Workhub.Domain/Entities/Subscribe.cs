namespace Workhub.Domain.Entities;

public class Subscribe
{
    public DateTime SubscribeOn { get; set; } = DateTime.UtcNow;
    public DateTime ExpireOn { get; set; } = DateTime.UtcNow;
    public bool IsSubscribed { get; set; } = false;
}
