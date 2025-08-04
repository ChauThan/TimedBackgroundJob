using System;

namespace TimedBackgroundJob
{
    /// <summary>
    /// Attribute to mark a class as a timed job for automatic discovery and registration.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TimedJobAttribute : Attribute
    {
        /// <summary>
        /// Interval between job executions.
        /// </summary>
        public TimeSpan Interval { get; }

        /// <summary>
        /// Prevent overlapping executions.
        /// </summary>
        public bool PreventOverlap { get; }

        /// <summary>
        /// Creates a new instance of <see cref="TimedJobAttribute"/>.
        /// </summary>
        /// <param name="intervalMinutes">Interval in minutes between job executions.</param>
        /// <param name="preventOverlap">Whether to prevent overlapping executions.</param>
        public TimedJobAttribute(int intervalMinutes = 60, bool preventOverlap = true)
        {
            Interval = TimeSpan.FromMinutes(intervalMinutes);
            PreventOverlap = preventOverlap;
        }
    }
}
