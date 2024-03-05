namespace Workhub.Domain.Entities;
public class Profile : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string NIN { get; set; } = string.Empty;
    public string ProfileImage { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Occupation { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Experience { get; set; } = string.Empty;
    public double Rating { get; set; } = 0.00;
    public int JobCount { get; set; } = 0;
    public Subscribe Subscribe { get; set; } = new Subscribe();
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
    public string LongLat { get; set; } = string.Empty;
    public string UserType { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

}
