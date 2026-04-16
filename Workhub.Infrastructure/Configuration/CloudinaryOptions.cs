namespace Workhub.Infrastructure.Configuration;

/// <summary>
/// Cloudinary configuration options mapped from appsettings.json
/// </summary>
public class CloudinaryOptions
{
    public const string SectionName = "Cloudinary";

    public string Cloud { get; set; } = string.Empty;
    public string Apikey { get; set; } = string.Empty;
    public string ApiSecret { get; set; } = string.Empty;
}
