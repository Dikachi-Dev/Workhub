using Serilog;
using Workhub.Application.Interfaces.Logger;

namespace Workhub.Infrastructure.GlobalLogger
{

    internal class Logger : ISeriLogger
    {
        private readonly ILogger logger;

        public Logger(ILogger logger)
        {
            logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        public void LogExceptions(string message, DateTime dateTime)
        {
            logger.Fatal(message, dateTime);
        }

        public void LogInError(string username, DateTime dateTime, string message)
        {
            logger.Error($"Login by {username} on {dateTime} Failed With Message {message}");
        }

        public void LogInformation(string username, DateTime dateTime)
        {
            logger.Information($"Login by {username}  on {dateTime} SuccessFul");
        }


    }
}
