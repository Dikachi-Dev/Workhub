
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
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

    public bool SendEmailAsyncMimeKit(string to, string subject, string body)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        string from = configuration.GetSection("Smtp:Email").Value;
        string displyname = configuration.GetSection("Smtp:DisplayName").Value;

        if (from != "")
        {
            foreach (string s in to.Split(','))
            {
                if (s != null && s.Trim() != "")
                {
                    MailMessage msg = new MailMessage();
                    msg.From = new MailAddress(from, displyname);
                    msg.Subject = subject;
                    msg.Body = body;
                    msg.To.Add(s.Trim());
                    msg.IsBodyHtml = true;

                    string SMTPServer =
                        configuration.GetSection("Smtp:Host").Value == null ||
                         configuration.GetSection("Smtp:Host").Value == ""
                            ? "smtp.ionos.co.uk"
                            : configuration.GetSection("Smtp:Host").Value;
                    string SMTPPassword =
                         configuration.GetSection("Smtp:Password").Value == null ||
                        configuration.GetSection("Smtp:Password").Value == ""
                            ? ""
                            : configuration.GetSection("Smtp:Password").Value;
                    string SMTPUserName =
                        configuration.GetSection("Smtp:Username").Value == null ||
                        configuration.GetSection("Smtp:Username").Value == ""
                            ? ""
                            : configuration.GetSection("Smtp:Username").Value;

                    SmtpClient smtpClient = new SmtpClient();
                    NetworkCredential basicCredential = new NetworkCredential(SMTPUserName, SMTPPassword);
                    smtpClient.Host = SMTPServer;
                    if (configuration.GetSection("Smtp:Port") != null &&
                        configuration.GetSection("Smtp:Port").Value.ToString() != "")
                        smtpClient.Port = int.Parse(configuration.GetSection("Smtp:Port").Value.ToString());
                    else
                        smtpClient.Port = 465;
                    if (configuration.GetSection("Smtp:EnableSSL") != null &&
                        configuration.GetSection("Smtp:EnableSSL").Value.ToString() != "")
                        smtpClient.EnableSsl = bool.Parse(configuration.GetSection("Smtp:EnableSSL").Value.ToString());

                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = basicCredential;
                    try
                    {
                        smtpClient.Send(msg);
                        return true;
                    }
                    catch (Exception sendexp)
                    {
                        Console.WriteLine(sendexp);
                        logger.LogExceptions($"Error sending email: {sendexp.Message}", DateTime.Now);
                        return false;
                    }
                }
            }
        }
        return false;
    }
}