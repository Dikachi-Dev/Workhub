namespace Workhub.Application.Interfaces.Logger;

public interface ISeriLogger
{
    void LogInformation(string username, DateTime dateTime);
    void LogInError(string username, DateTime dateTime, string message);
    void LogExceptions(string message, DateTime dateTime);
}
