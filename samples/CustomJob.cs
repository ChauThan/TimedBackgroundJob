using TimedBackgroundJob;
using System.Threading;
using System.Threading.Tasks;

namespace Samples
{
    public class CustomJob : ITimedJob
    {
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // Example job logic: log, process, etc.
            await Task.Delay(1000, cancellationToken);
        }
    }
}
