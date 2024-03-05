namespace Workhub.Domain.Entities;

public class ChatPost : BaseEntity
{
    public string SenderId { get; set; } = String.Empty;
    public string ReceiverId { get; set; } = String.Empty;
    public ICollection<Reply> Replys { get; set; } = new List<Reply>();

}
