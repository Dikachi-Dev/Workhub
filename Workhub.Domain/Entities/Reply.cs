namespace Workhub.Domain.Entities;

public class Reply : BaseEntity
{
    public ChatPost ChatPost { get; set; } = new ChatPost();
    public string Message { get; set; } = String.Empty;
    public string FromId { get; set; } = String.Empty;
}