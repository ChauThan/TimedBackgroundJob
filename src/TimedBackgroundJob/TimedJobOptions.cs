namespace TimedBackgroundJob
{
    /// <summary>
    /// Options for configuring a timed job.
    /// </summary>
    public class TimedJobOptions
    {
        /// <summary>
        /// Interval between job executions.
        /// </summary>
        public TimeSpan Interval { get; set; } = TimeSpan.FromHours(1);

        /// <summary>
        /// Optional: Prevent overlapping executions.
        /// </summary>
        public bool PreventOverlap { get; set; } = true;
    }
}
