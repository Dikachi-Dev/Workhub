namespace Workhub.Domain.Entities;

public class Subscribe
{
    public DateTime SubscribeOn { get; set; } = DateTime.UtcNow.Date;
    public DateTime ExpireOn { get; set; } = DateTime.UtcNow.Date;
    public bool IsSubscribed { get; set; } = false;
}
