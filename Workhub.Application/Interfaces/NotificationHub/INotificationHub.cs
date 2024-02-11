namespace Workhub.Application.Interfaces.NotificationHub
{
    public interface INotificationHub
    {
        Task SendMessage(string user, string message, string title);
    }
}
