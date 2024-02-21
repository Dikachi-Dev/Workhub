namespace Workhub.Domain.Entities;

public class Reply : BaseEntity
{
    public string Message { get; set; } = String.Empty;
    public string FromId { get; set; } = String.Empty;
}