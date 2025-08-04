using System;
using System.Threading;
using System.Threading.Tasks;
using TimedBackgroundJob;

namespace TimedBackgroundJob.Example
{
    [TimedJob(15, true)]
    public class TimeLoggerJob : ITimedJob
    {
        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Current time logged: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            return Task.CompletedTask;
        }
    }
}
