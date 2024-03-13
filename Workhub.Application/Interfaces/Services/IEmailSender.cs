namespace Workhub.Application.Interfaces.Services;
public interface IEmailSender {
    Task<bool> SendEmailAsyncMimeKit(string to, string subject, string body);
}