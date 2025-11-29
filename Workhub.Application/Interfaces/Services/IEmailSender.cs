namespace Workhub.Application.Interfaces.Services;
public interface IEmailSender {
    Task<bool> SendEmailAsync(string to, string subject, string body);
}