namespace Workhub.Application.Interfaces.Services;
public interface IEmailSender {
    bool SendEmailAsync(string to, string subject, string body);
}