using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TimedBackgroundJob
{
    /// <summary>
    /// Hosted service that runs timed jobs at configured intervals.
    /// </summary>
    public class TimedJobHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimedJobRegistry _registry;
        private readonly ILogger<TimedJobHostedService> _logger;
        private readonly Dictionary<Type, bool> _jobRunning = new();


        /// <summary>
        /// Initializes a new instance of the <see cref="TimedJobHostedService"/> class.
        /// </summary>
        /// <param name="scopeFactory">Factory for creating service scopes.</param>
        /// <param name="registry">Registry containing job registrations.</param>
        /// <param name="logger">Logger for error and status reporting.</param>
        public TimedJobHostedService(IServiceScopeFactory scopeFactory, TimedJobRegistry registry, ILogger<TimedJobHostedService> logger)
        {
            _scopeFactory = scopeFactory;
            _registry = registry;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var tasks = new List<Task>();
            foreach (var registration in _registry.Registrations)
            {
                tasks.Add(RunJobAsync(registration, stoppingToken));
            }
            await Task.WhenAll(tasks);
        }

        private async Task RunJobAsync(TimedJobRegistration registration, CancellationToken stoppingToken)
        {
            var options = registration.Options;
            _jobRunning[registration.JobType] = false;
            while (!stoppingToken.IsCancellationRequested)
            {
                if (options.PreventOverlap && _jobRunning[registration.JobType])
                {
                    await Task.Delay(options.Interval, stoppingToken);
                    continue;
                }
                _jobRunning[registration.JobType] = true;
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var job = (ITimedJob)scope.ServiceProvider.GetRequiredService(registration.JobType);
                    await job.ExecuteAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    // Log error with job type and exception details for diagnostics and monitoring
                    _logger.LogError(ex, "Error executing timed job {JobType}", registration.JobType.FullName);
                }
                finally
                {
                    _jobRunning[registration.JobType] = false;
                }
                await Task.Delay(options.Interval, stoppingToken);
            }
        }
    }
}
