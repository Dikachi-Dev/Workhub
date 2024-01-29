using Workhub.Application.Interfaces.Logger;

namespace Workhub.Infrastructure.GlobalLogger
{

    internal class SeriLogger : ISeriLogger
    {
        private readonly Serilog.ILogger logger;

        public SeriLogger(Serilog.ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            logger.Information($"Login by {username} on {dateTime} Successful");
        }
    }
}
