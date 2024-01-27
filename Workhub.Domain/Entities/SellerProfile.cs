namespace Workhub.Domain.Entities;
public class SellerProfile : BaseEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Occupation { get; set; }
    public string? Gender { get; set; }
    public string? Experience { get; set; }
    public string? Password { get; set; }
}