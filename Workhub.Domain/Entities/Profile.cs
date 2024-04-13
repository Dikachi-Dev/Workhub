namespace Workhub.Domain.Entities;
public class Profile : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string NIN { get; set; } = string.Empty;
    public ImageDet ProfileImage { get; set; } = new ImageDet();
    public string Country { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Occupation { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Experience { get; set; } = string.Empty;
    public int Rating { get; set; } = 0;
    public int JobCount { get; set; } = 0;
    public Subscribe Subscribe { get; set; } = new Subscribe();
    public VendorProfile VendorProfile {get; set;} = new VendorProfile();
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
    public string Token {get; set;} = string.Empty;
    public string LongLat { get; set; } = string.Empty;
    public string UserType { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

}

public class ImageDet{
    public string Description { get; set; } = string.Empty;
    public string publicId { get; set; } = string.Empty;
}