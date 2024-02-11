using Microsoft.AspNetCore.SignalR;
using Workhub.Application.Interfaces.NotificationHub;

namespace Workhub.Infrastructure.Notification;

public class NotificationHub : Hub<INotificationHub>
{
    public async Task SendMessage(string user, string message, string title)
    {
        await Clients.All.SendMessage(title, user, message);
    }
}
