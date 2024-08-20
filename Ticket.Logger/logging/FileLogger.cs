using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Ticket.logging
{
    public class FileLogger : ILogger
    {
        private static readonly object _lock = new object();
        readonly FileLoggerProvider _loggerProvider;
        private readonly string _logFilePath;

        public FileLogger([NotNull] FileLoggerProvider loggerProvider)
        {
            _loggerProvider = loggerProvider;
            _logFilePath = string.Format("{0}/{1}", _loggerProvider.Options.FolderPath,
                                         _loggerProvider.Options.FilePath.Replace("{date}", DateTime.Now.ToString("yyyyMMdd-HHmmss")));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var logRecord = string.Format("{0} [{1}] {2} {3}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                logLevel.ToString(),
                formatter(state, exception),
                exception != null ? exception.StackTrace : "");

            Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath));

            lock (_lock)
            {
                using (var streamWriter = new StreamWriter(_logFilePath, true))
                {
                    streamWriter.WriteLine(logRecord);
                }
            }
        }
    }
}
