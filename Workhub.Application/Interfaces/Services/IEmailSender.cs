namespace Workhub.Application.Interfaces.Services;
public interface IEmailSender {
    bool SendEmailAsyncMimeKit(string to, string subject, string body);
}