using FirebaseAdmin.Messaging;
using Workhub.Application.Interfaces.Logger;
using Workhub.Application.Interfaces.Services;

namespace Workhub.Infrastructure.Services;
public class NotificationSender : INotificationSender
{
    private readonly ISeriLogger logger;

    public NotificationSender(ISeriLogger logger)
    {
        this.logger = logger;
    }

    public async Task SendFcmMessage(string token, string title, string body, string datatitle)
    {
        var message = new Message()
        {
            Token = token,
            Notification = new FirebaseAdmin.Messaging.Notification(){
                Title = title
            },
            Data = new Dictionary<string, string>
                {
                    { datatitle, body }
                }
        };


        try
        {
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message).ConfigureAwait(true);
           logger.LogInfo(response,DateTime.UtcNow);
        }
        catch (FirebaseMessagingException ex)
        {
            logger.LogExceptions(ex.Message, DateTime.UtcNow);
            
        }
    }
}