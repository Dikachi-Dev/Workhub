namespace Workhub.Domain.Entities;
public class SellerProfile : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string NIN { get; set; } = string.Empty;
    public string ProfileImage { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Occupation { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Experience { get; set; } = string.Empty;
    public double Rating { get; set; }
    public int JobCount { get; set; }
    public string Password { get; set; } = string.Empty;
}