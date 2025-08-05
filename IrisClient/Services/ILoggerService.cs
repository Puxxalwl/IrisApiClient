using System;

namespace IrisClient.Services
{
    public interface ILoggerService
    {
        void LogInformation(string Message);
        void LogError(string Message, Exception? Err = null);
        void LogWarn(string Message);
    }
}

