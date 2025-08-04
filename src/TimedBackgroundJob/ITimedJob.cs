namespace TimedBackgroundJob
{
    /// <summary>
    /// Interface for timed background jobs.
    /// </summary>
    public interface ITimedJob
    {
        /// <summary>
        /// Executes the job logic.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for graceful shutdown.</param>
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
