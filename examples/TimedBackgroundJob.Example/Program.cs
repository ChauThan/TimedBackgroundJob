using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TimedBackgroundJob.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Manual registration example
                    services.AddTimedJob<HelloWorldJob>(options =>
                    {
                        options.Interval = TimeSpan.FromSeconds(10);
                    });
                    // Register a new job manually
                    services.AddTimedJob<ManualJob>(options =>
                    {
                        options.Interval = TimeSpan.FromSeconds(20);
                    });
                    // Delegate-based registration example
                    services.AddTimedJob(options =>
                    {
                        options.Interval = TimeSpan.FromSeconds(15);
                    },
                    (serviceProvider) =>
                    {
                        var loggerFactory = (Microsoft.Extensions.Logging.ILoggerFactory)serviceProvider.GetService(typeof(Microsoft.Extensions.Logging.ILoggerFactory));
                        var logger = loggerFactory != null ? loggerFactory.CreateLogger("DelegateJob") : null;
                        if (logger != null)
                        {
                            ((Microsoft.Extensions.Logging.ILogger)logger).LogInformation($"Delegate job executed at {DateTime.Now:HH:mm:ss}");
                        }
                        return System.Threading.Tasks.Task.CompletedTask;
                    });
                    // Attribute-based registration example
                    services.AddTimedJobsFromAttributes(new[] { typeof(Program).Assembly });
                })
                .Build();

            host.Run();
        }
    }
}
