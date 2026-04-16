
namespace Workhub.Domain.Entities;

public class Otp
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = string.Empty;
    public DateTime ExpiryTime { get; set; } = DateTime.UtcNow.AddMinutes(5);
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Email { get; set; } = string.Empty;
}

