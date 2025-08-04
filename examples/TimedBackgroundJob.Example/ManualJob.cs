using System;
using System.Threading;
using System.Threading.Tasks;
using TimedBackgroundJob;

namespace TimedBackgroundJob.Example
{
    /// <summary>
    /// A manually registered job for demonstration purposes.
    /// </summary>
    public class ManualJob : ITimedJob
    {
        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"ManualJob executed at {DateTime.Now:HH:mm:ss}");
            return Task.CompletedTask;
        }
    }
}
