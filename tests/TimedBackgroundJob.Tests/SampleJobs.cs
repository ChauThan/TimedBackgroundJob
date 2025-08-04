namespace TimedBackgroundJob.Tests
{
    public class CustomJob : ITimedJob
    {
        public bool Executed { get; private set; }
        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Executed = true;
            return Task.CompletedTask;
        }
    }

    public class LongRunningJob : ITimedJob
    {
        public int ExecutionCount { get; private set; }
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            ExecutionCount++;
            await Task.Delay(200, cancellationToken); // Simulate long-running job
        }
    }

    public class FailingJob : ITimedJob
    {
        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            throw new Exception("Job failed");
        }
    }
}
