namespace TimedBackgroundJob
{
    public class TimedJobRegistration
    {
        public Type JobType { get; }
        public TimedJobOptions Options { get; }
        public TimedJobRegistration(Type jobType, TimedJobOptions options)
        {
            JobType = jobType;
            Options = options;
        }
    }

    public class TimedJobRegistry
    {
        private readonly List<TimedJobRegistration> _registrations;

        public TimedJobRegistry(IEnumerable<TimedJobRegistration> registrations)
        {
            _registrations = [.. registrations];
        }

        public void Add(TimedJobRegistration registration) => _registrations.Add(registration);
        public IReadOnlyList<TimedJobRegistration> Registrations => _registrations;
    }
}
