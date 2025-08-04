using System;
using System.Threading;
using System.Threading.Tasks;
using TimedBackgroundJob;

namespace TimedBackgroundJob.Example
{
    public class HelloWorldJob : ITimedJob
    {
        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Hello, World! The time is {DateTime.Now:HH:mm:ss}");
            return Task.CompletedTask;
        }
    }
}
