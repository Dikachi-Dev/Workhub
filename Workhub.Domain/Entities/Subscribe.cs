namespace Workhub.Domain.Entities;

public class Subscribe
{
    public DateTime SubscribeOn { get; set; }
    public DateTime ExpireOn { get; set; }
    public bool IsSubscribed { get; set; }
}
