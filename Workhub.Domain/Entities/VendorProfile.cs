namespace Workhub.Domain.Entities;

public class VendorProfile
{
    public string Description { get; set; } = string.Empty;
    public string Image1 { get; set; } = string.Empty;
    public string Image2 { get; set; } = string.Empty;
    public string Instagram { get; set; } = string.Empty;

    public void UpdateDetails(string description, string image1, string image2, string instagram)
    {
        Description = description;
        Image1 = image1;
        Image2 = image2;
        Instagram = instagram;
    }

    public void UpdateDescription(string description)
    {
        Description = description;
    }

    public void UpdateImages(string image1, string image2, string description, string instagram)
    {
        Image1 = image1;
        Image2 = image2;
        Description = description;
        Instagram = instagram;
    }
}
