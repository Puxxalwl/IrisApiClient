using IrisClient.Services;
using Microsoft.Extensions.Logging;
using System;

namespace IrisClient.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;

        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string Message) => _logger.LogInformation(Message);
        public void LogError(string Message, Exception? Err = null) => _logger.LogError(Err, Message);
        public void LogWarn(string Message) => _logger.LogWarning(Message);
    }
}

