namespace Workhub.Domain.Entities;

public class ChatPost : BaseEntity
{
    public string SenderId { get; set; } = String.Empty;
    public string ReceiverId { get; set; } = String.Empty;
    public string SenderName { get; set; } = String.Empty;
    public string ReceiverName { get; set; } = String.Empty;
    public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
    public ICollection<Reply> Replys { get; set; } = new List<Reply>();

}
