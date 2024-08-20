using Microsoft.Extensions.Options;
using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Ticket.logging
{
    [ProviderAlias("TicketFile")]
    public class FileLoggerProvider : ILoggerProvider
    {
        public readonly FileLoggerOptions Options;

        public FileLoggerProvider(IOptions<FileLoggerOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Options = options.Value ?? throw new ArgumentNullException(nameof(options.Value));

            if (string.IsNullOrWhiteSpace(Options.FolderPath))
            {
                throw new ArgumentNullException(nameof(Options.FolderPath), "FolderPath cannot be null or empty.");
            }

            if (!Directory.Exists(Options.FolderPath))
            {
                Directory.CreateDirectory(Options.FolderPath);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(this);
        }

        public void Dispose()
        {
        }
    }
}