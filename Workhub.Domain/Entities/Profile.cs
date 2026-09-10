using NetTopologySuite.Geometries;

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
    public int Rating { get; set; } = 0;
    public int JobCount { get; set; } = 0;
    public Subscribe Subscribe { get; set; } = new Subscribe();
    public VendorProfile VendorProfile { get; set; } = new VendorProfile();
    public string Token { get; set; } = string.Empty;
    public string LongLat { get; set; } = string.Empty; // Deprecated: Use Location instead
    public Point? Location { get; set; } // PostGIS Point (longitude, latitude)
    public string UserType { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool isDeleted { get; set; } = false;

    public void UpdateRating(int newRating)
    {
        Rating = Math.Clamp(newRating, 0, 5);
    }

    public void UpdateLocation(Point location, string? longLat = null, string? country = null, string? state = null, string? address = null)
    {
        Location = location;
        if (!string.IsNullOrWhiteSpace(longLat)) LongLat = longLat;
        if (!string.IsNullOrWhiteSpace(country)) Country = country;
        if (!string.IsNullOrWhiteSpace(state)) State = state;
        if (!string.IsNullOrWhiteSpace(address)) Address = address;
    }

    public void UpdatePersonalInfo(string firstName, string lastName, string phoneNumber, string? profileImage = null)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        if (!string.IsNullOrWhiteSpace(profileImage))
        {
            ProfileImage = profileImage;
        }
    }

    public void SoftDelete()
    {
        isDeleted = true;
    }
}