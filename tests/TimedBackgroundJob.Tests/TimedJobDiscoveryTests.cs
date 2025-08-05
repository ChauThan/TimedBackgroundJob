using Microsoft.Extensions.DependencyInjection;
using TimedBackgroundJob;

namespace TimedBackgroundJob.Tests
{
    public class TimedJobDiscoveryTests
    {
        [Fact]
        public void DoesNotRegisterDuplicateJobs_WhenManualAndAttributeUsed()
        {
            var services = new ServiceCollection();
            // Manual registration
            services.AddTimedJob<SampleJob>(options =>
            {
                options.Interval = TimeSpan.FromMinutes(15);
                options.PreventOverlap = false;
            });
            // Attribute-based registration
            services.AddTimedJobsFromAttributes(new[] { typeof(SampleJob).Assembly });
            var provider = services.BuildServiceProvider();
            var registry = provider.GetService<TimedJobRegistry>();
            Assert.NotNull(registry);
            // Should only be one registration for SampleJob
            Assert.Equal(1, registry.Registrations.Count(r => r is TypeJobRegistration tjr && tjr.JobType == typeof(SampleJob)));
        }

        [Fact]
        public void DiscoversAndRegistersJobsWithAttribute()
        {
            var services = new ServiceCollection();
            // Only attribute-based registration
            services.AddTimedJobsFromAttributes(new[] { typeof(SampleJob).Assembly });
            var provider = services.BuildServiceProvider();
            var registry = provider.GetService<TimedJobRegistry>();
            Assert.NotNull(registry);
            Assert.Contains(registry.Registrations, r => r is TypeJobRegistration tjr && tjr.JobType == typeof(SampleJob));
        }

        [Fact]
        public void MapsAttributeParametersToOptions()
        {
            var services = new ServiceCollection();
            // Only attribute-based registration
            services.AddTimedJobsFromAttributes(new[] { typeof(SampleJob).Assembly });
            var provider = services.BuildServiceProvider();
            var registry = provider.GetService<TimedJobRegistry>();
            var reg = registry?.Registrations.FirstOrDefault(r => r is TypeJobRegistration tjr && tjr.JobType == typeof(SampleJob)) as TypeJobRegistration;
            Assert.Equal(TimeSpan.FromMinutes(15), reg?.Options.Interval);
            Assert.False(reg?.Options.PreventOverlap);
        }
    }
}

[TimedJob(15, false)]
public class SampleJob : ITimedJob
{
    public Task ExecuteAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
