namespace Workhub.Application.Interfaces.Services;
public interface INotificationSender 
{
    Task SendFcmMessage(string token, string title, string body, string datatitle);
}