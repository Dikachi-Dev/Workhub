namespace Workhub.Infrastructure.Configuration;

/// <summary>
/// SMTP configuration options mapped from appsettings.json
/// </summary>
public class SmtpOptions
{
    public const string SectionName = "Smtp";

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Port { get; set; }
    public bool EnableSSL { get; set; }
    public string DisplayName { get; set; } = string.Empty;
}
