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
        private readonly Dictionary<string, bool> _jobRunning = new();


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
            _jobRunning[registration.Id] = false;
            while (!stoppingToken.IsCancellationRequested)
            {
                if (options.PreventOverlap && _jobRunning[registration.Id])
                {
                    await Task.Delay(options.Interval, stoppingToken);
                    continue;
                }
                _jobRunning[registration.Id] = true;
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    await registration.ExecuteAsync(scope.ServiceProvider, stoppingToken);
                }
                catch (Exception ex)
                {
                    // Log error with job type and exception details for diagnostics and monitoring
                    if (registration is TypeJobRegistration typeReg)
                    {
                        _logger.LogError(ex, "Error executing timed job {JobType}", typeReg.JobType.FullName);
                    }
                    else if (registration is DelegateJobRegistration)
                    {
                        _logger.LogError(ex, "Error executing delegate-based timed job");
                    }
                    else
                    {
                        _logger.LogError(ex, "Error executing unknown job type");
                    }
                }
                finally
                {
                    _jobRunning[registration.Id] = false;
                }
                await Task.Delay(options.Interval, stoppingToken);
            }
        }
    }
}
