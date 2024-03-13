
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Workhub.Application.Interfaces.Logger;
using Workhub.Application.Interfaces.Services;

namespace Workhub.Infrastructure.Services;

public class EmailSender : IEmailSender
{
    private readonly ISeriLogger logger;


    public EmailSender(ISeriLogger logger)
    {
        this.logger = logger;
      
    }

    public async Task<bool> SendEmailAsyncMimeKit(string to, string subject, string body)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        try
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(configuration.GetSection("Email").Value);
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(configuration.GetSection("Host").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(configuration.GetSection("Username").Value,configuration.GetSection("Password").Value);
            await smtp.SendAsync(email);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogExceptions($"Error sending email: {ex.Message}", DateTime.Now);
            return false;
        }
    }
}