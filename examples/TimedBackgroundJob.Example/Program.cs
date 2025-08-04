using System;
using Microsoft.Extensions.Hosting;

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
                    // Attribute-based registration example
                    services.AddTimedJobsFromAttributes(new[] { typeof(Program).Assembly });
                })
                .Build();

            host.Run();
        }
    }
}
