using System;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace TimedBackgroundJob
{
    public abstract class TimedJobRegistration
    {
        public TimedJobOptions Options { get; }
        public string Id { get; }
        protected TimedJobRegistration(TimedJobOptions options, string id)
        {
            Options = options;
            Id = id;
        }
        public abstract Task ExecuteAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }

    public class TypeJobRegistration : TimedJobRegistration
    {
        public Type JobType { get; }
        public TypeJobRegistration(Type jobType, TimedJobOptions options)
            : base(options, jobType.FullName ?? Guid.NewGuid().ToString())
        {
            JobType = jobType;
        }
        public override async Task ExecuteAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var job = (ITimedJob)serviceProvider.GetRequiredService(JobType);
            await job.ExecuteAsync(cancellationToken);
        }
    }

    public class DelegateJobRegistration : TimedJobRegistration
    {
        public Func<IServiceProvider, Task> JobDelegate { get; }
        public DelegateJobRegistration(Func<IServiceProvider, Task> jobDelegate, TimedJobOptions options)
            : base(options, Guid.NewGuid().ToString())
        {
            JobDelegate = jobDelegate;
        }
        public override async Task ExecuteAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            await JobDelegate(serviceProvider);
        }
    }

    public class TimedJobRegistry
    {
        private readonly List<TimedJobRegistration> _registrations;

        public TimedJobRegistry(IEnumerable<TimedJobRegistration> registrations)
        {
            _registrations = new List<TimedJobRegistration>(registrations);
        }

        public void Add(TimedJobRegistration registration) => _registrations.Add(registration);
        public IReadOnlyList<TimedJobRegistration> Registrations => _registrations;
    }
}

