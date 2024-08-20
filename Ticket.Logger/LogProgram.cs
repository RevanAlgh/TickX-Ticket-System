using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ticket.logging;

class LogProgram
{
    static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging(builder =>
        {
            builder.AddFileLogger(options =>
            {
                options.FolderPath = "logs";
                options.FilePath = "log-{date}.txt";
            });
        });

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var logger = serviceProvider.GetService<ILogger<LogProgram>>();
    }
}
